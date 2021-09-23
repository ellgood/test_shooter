using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CommonLayer.UserInterface.Utility
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasHelper : MonoBehaviour
    {
        public static readonly UnityEvent OnOrientationChange = new UnityEvent();
        public static readonly UnityEvent OnResolutionChange = new UnityEvent();

        private static readonly List<CanvasHelper> Helpers = new List<CanvasHelper>();

        private static bool screenChangeVarsInitialized;
        private static ScreenOrientation lastOrientation = ScreenOrientation.Portrait;
        private static Vector2 lastResolution = Vector2.zero;
        private static Rect lastSafeArea = Rect.zero;

        private Canvas _canvas;
        private RectTransform _rectTransform;

        [SerializeField]
        private RectTransform _safeAreaTransform;
        public static bool IsLandscape { get; private set; }

        public static Vector2 GetCanvasSize()
        {
            return Helpers[0]._rectTransform.sizeDelta;
        }

        public static Vector2 GetSafeAreaSize()
        {
            foreach (CanvasHelper helper in Helpers.Where(helper => helper._safeAreaTransform != null))
            {
                return helper._safeAreaTransform.sizeDelta;
            }

            return GetCanvasSize();
        }

        private void Awake()
        {
            if (!Helpers.Contains(this))
            {
                Helpers.Add(this);
            }

            _canvas = GetComponent<Canvas>();
            _rectTransform = GetComponent<RectTransform>();

            if (!_safeAreaTransform)
            {
                _safeAreaTransform = transform.Find("SafeArea") as RectTransform;
            }

            if (screenChangeVarsInitialized)
            {
                return;
            }

            lastOrientation = Screen.orientation;
            lastResolution.x = Screen.width;
            lastResolution.y = Screen.height;
            lastSafeArea = Screen.safeArea;

            screenChangeVarsInitialized = true;
        }

        private void Start()
        {
            ApplySafeArea();
        }

        private void Update()
        {
            if (Helpers[0] != this)
            {
                return;
            }

            if (Screen.orientation != lastOrientation)
            {
                OrientationChanged();
            }

            if (Screen.safeArea != lastSafeArea)
            {
                SafeAreaChanged();
            }

            if (Math.Abs(Screen.width - lastResolution.x) > 1 || Math.Abs(Screen.height - lastResolution.y) > 1)
            {
                ResolutionChanged();
            }
        }

        private void ApplySafeArea()
        {
            if (_safeAreaTransform == null)
            {
                return;
            }

            Rect safeArea = Screen.safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            
            Rect pixelRect = _canvas.pixelRect;
            anchorMin.x /= pixelRect.width;
            anchorMin.y /= pixelRect.height;
            anchorMax.x /= pixelRect.width;
            anchorMax.y /= pixelRect.height;

            _safeAreaTransform.anchorMin = anchorMin;
            _safeAreaTransform.anchorMax = anchorMax;

            // Debug.Log(
            // "ApplySafeArea:" +
            // "\n Screen.orientation: " + Screen.orientation +
            // #if UNITY_IOS
            // "\n Device.generation: " + UnityEngine.iOS.Device.generation.ToString() +
            // #endif
            // "\n Screen.safeArea.position: " + Screen.safeArea.position.ToString() +
            // "\n Screen.safeArea.size: " + Screen.safeArea.size.ToString() +
            // "\n Screen.width / height: (" + Screen.width.ToString() + ", " + Screen.height.ToString() + ")" +
            // "\n canvas.pixelRect.size: " + canvas.pixelRect.size.ToString() +
            // "\n anchorMin: " + anchorMin.ToString() +
            // "\n anchorMax: " + anchorMax.ToString());
        }

        private void OnDestroy()
        {
            if (Helpers != null && Helpers.Contains(this))
            {
                Helpers.Remove(this);
            }
        }

        private static void OrientationChanged()
        {
            //Debug.Log("Orientation changed from " + lastOrientation + " to " + Screen.orientation + " at " + Time.time);

            lastOrientation = Screen.orientation;
            lastResolution.x = Screen.width;
            lastResolution.y = Screen.height;

            IsLandscape = lastOrientation == ScreenOrientation.LandscapeLeft || lastOrientation == ScreenOrientation.LandscapeRight ||
                          lastOrientation == ScreenOrientation.Landscape;
            OnOrientationChange.Invoke();
        }

        private static void ResolutionChanged()
        {
            if (Math.Abs(lastResolution.x - Screen.width) < 1 && Math.Abs(lastResolution.y - Screen.height) < 1)
            {
                return;
            }

            //Debug.Log("Resolution changed from " + lastResolution + " to (" + Screen.width + ", " + Screen.height + ") at " + Time.time);

            lastResolution.x = Screen.width;
            lastResolution.y = Screen.height;

            IsLandscape = Screen.width > Screen.height;
            OnResolutionChange.Invoke();
        }

        private static void SafeAreaChanged()
        {
            if (lastSafeArea == Screen.safeArea)
            {
                return;
            }

            //Debug.Log("Safe Area changed from " + lastSafeArea + " to " + Screen.safeArea.size + " at " + Time.time);

            lastSafeArea = Screen.safeArea;

            foreach (CanvasHelper helper in Helpers)
            {
                helper.ApplySafeArea();
            }
        }
    }
}