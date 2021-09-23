using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace CommonLayer.UserInterface.Utility
{
   public static class RectTransformUtils
    {
        /// <summary>
        ///     CResize the rect relative to the screen size
        /// </summary>
        /// <param name="rect">Target rect transform</param>
        /// <param name="screenWidth">Screen Width</param>
        /// <param name="screenHeight">Screen Height</param>
        /// <param name="referenceResolution">Reference Resolution</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReScaleHeightRect(
            this RectTransform rect,
            float screenHeight,
            float screenWidth,
            Vector2 referenceResolution)
        {
            float refDiv = referenceResolution.x / referenceResolution.y;
            float screenDiv = screenWidth / screenHeight;

            float scale = 1 * refDiv / screenDiv;

            rect.localScale = new Vector3(scale, scale);
        }

        /// <summary>
        ///     Convert event coordinates to local rectangle
        /// </summary>
        /// <param name="rect">Target rect transform</param>
        /// <param name="pointerData">Target pointer event data</param>
        /// <returns></returns>
        public static Vector2 ConvertPointerPosition(this RectTransform rect, PointerEventData pointerData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, pointerData.position,
                                                                    pointerData.pressEventCamera, out Vector2 position);

            return position;
        }

        /// <summary>
        ///     Set full size
        /// </summary>
        public static RectTransform SetFullSize(this RectTransform self)
        {
            self.sizeDelta = new Vector2(0.0f, 0.0f);
            self.anchorMin = new Vector2(0.0f, 0.0f);
            self.anchorMax = new Vector2(1.0f, 1.0f);
            self.pivot = new Vector2(0.5f, 0.5f);
            return self;
        }

        /// <summary>
        ///     Get rect size
        /// </summary>
        public static Vector2 GetSize(this RectTransform target)
        {
            return target.rect.size;
        }

        public static float GetSizeX(this RectTransform rect)
        {
            return rect.rect.size.x;
        }

        public static float GetSizeY(this RectTransform rect)
        {
            return rect.rect.size.y;
        }

        /// <summary>
        ///     Set rect size
        /// </summary>
        public static void SetSize(this RectTransform target, Vector2 newSize)
        {
            Vector2 pivot = target.pivot;
            Vector2 dist = newSize - target.rect.size;
            target.offsetMin -= new Vector2(dist.x * pivot.x, dist.y * pivot.y);
            target.offsetMax += new Vector2(dist.x * (1f - pivot.x), dist.y * (1f - pivot.y));
        }

        /// <summary>
        ///     Set left anchor relative size
        /// </summary>
        public static RectTransform SetSizeFromLeft(this RectTransform target, float rate)
        {
            target.SetFullSize();

            float width = target.rect.width;

            target.anchorMin = new Vector2(0.0f, 0.0f);
            target.anchorMax = new Vector2(0.0f, 1.0f);
            target.pivot = new Vector2(0.0f, 1.0f);
            target.sizeDelta = new Vector2(width * rate, 0.0f);

            return target;
        }

        /// <summary>
        ///     Set right anchor relative size
        /// </summary>
        public static RectTransform SetSizeFromRight(this RectTransform target, float rate)
        {
            target.SetFullSize();

            float width = target.rect.width;

            target.anchorMin = new Vector2(1.0f, 0.0f);
            target.anchorMax = new Vector2(1.0f, 1.0f);
            target.pivot = new Vector2(1.0f, 1.0f);
            target.sizeDelta = new Vector2(width * rate, 0.0f);

            return target;
        }

        /// <summary>
        ///     Set top anchor relative size
        /// </summary>
        public static RectTransform SetSizeFromTop(this RectTransform target, float rate)
        {
            target.SetFullSize();

            float height = target.rect.height;

            target.anchorMin = new Vector2(0.0f, 1.0f);
            target.anchorMax = new Vector2(1.0f, 1.0f);
            target.pivot = new Vector2(0.0f, 1.0f);
            target.sizeDelta = new Vector2(0.0f, height * rate);

            return target;
        }

        /// <summary>
        ///     Set bottom anchor relative size
        /// </summary>
        public static RectTransform SetSizeFromBottom(this RectTransform target, float rate)
        {
            target.SetFullSize();

            float height = target.rect.height;

            target.anchorMin = new Vector2(0.0f, 0.0f);
            target.anchorMax = new Vector2(1.0f, 0.0f);
            target.pivot = new Vector2(0.0f, 0.0f);
            target.sizeDelta = new Vector2(0.0f, height * rate);

            return target;
        }

        /// <summary>
        ///     Set side based offset
        /// </summary>
        public static void SetOffset(
            this RectTransform self,
            float left,
            float top,
            float right,
            float bottom)
        {
            self.offsetMin = new Vector2(left, top);
            self.offsetMax = new Vector2(right, bottom);
        }

        /// <summary>
        ///     Checks on contains point in rectangle
        /// </summary>
        public static bool Contains(this RectTransform self, Vector2 screenPos)
        {
            var canvas = self.GetComponentInParent<Canvas>();
            switch (canvas.renderMode)
            {
                case RenderMode.ScreenSpaceCamera:
                {
                    Camera camera = canvas.worldCamera;
                    if (camera != null)
                    {
                        return RectTransformUtility.RectangleContainsScreenPoint(self, screenPos, camera);
                    }
                }
                    break;
                case RenderMode.ScreenSpaceOverlay:
                    return RectTransformUtility.RectangleContainsScreenPoint(self, screenPos);
                case RenderMode.WorldSpace:
                    return RectTransformUtility.RectangleContainsScreenPoint(self, screenPos);
            }

            return false;
        }

        /// <summary>
        ///     Checks on overlaps two rect transforms
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Overlaps(this RectTransform targetA, RectTransform targetB, Camera camera)
        {
            Assert.IsNotNull(targetA);
            Assert.IsNotNull(targetB);
            Assert.IsNotNull(camera);

            Rect rect1 = GetScreenRect(targetA, camera);
            Rect rect2 = GetScreenRect(targetB, camera);
            return rect1.Overlaps(rect2);
        }

        /// <summary>
        ///     Get screen coordinates
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect GetScreenRect(this RectTransform target)
        {
            Assert.IsNotNull(target);

            var canvas = target.GetComponentInParent<Canvas>();

            return canvas ? GetScreenRect(target, canvas) : Rect.zero;
        }

        /// <summary>
        ///     Get screen coordinates
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect GetScreenRectByRootCanvas(this RectTransform target)
        {
            Assert.IsNotNull(target);

            var canvas = target.GetComponentInParent<Canvas>();

            return canvas ? GetScreenRect(target, canvas.rootCanvas) : Rect.zero;
        }

        /// <summary>
        ///     Get screen coordinates
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect GetScreenRect(this RectTransform target, Canvas canvas)
        {
            Assert.IsNotNull(target);
            Assert.IsNotNull(canvas);

            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                var corners = new Vector3[4];
                target.GetWorldCorners(corners);

                float scaleFactor = canvas.scaleFactor;
                var rect = new Rect
                {
                    min = corners[0], // / scaleFactor,
                    max = corners[2] // / scaleFactor
                };

                return rect;
            }

            Camera camera = canvas.worldCamera;
            return camera ? GetScreenRect(target, camera) : Rect.zero;
        }

        /// <summary>
        ///     Get screen coordinates
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect GetScreenRect(this RectTransform target, Camera camera)
        {
            Assert.IsNotNull(camera);
            Assert.IsNotNull(target);

            var corners = new Vector3[4];
            target.GetWorldCorners(corners);
            var rect = new Rect
            {
                min = camera.WorldToScreenPoint(corners[0]),
                max = camera.WorldToScreenPoint(corners[2])
            };

            return rect;
        }

        public static Rect RectTransformToScreenSpace(RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            var rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
            rect.x -= transform.pivot.x * size.x;
            rect.y -= (1.0f - transform.pivot.y) * size.y;
            return rect;
        }

        /// <summary>
        ///     Get screen coordinates based on <see cref="Transform.localScale" />
        /// </summary>
        /// <param name="transform">Target rect transform</param>
        /// <returns>Rect based on lossy scale</returns>
        public static Rect GetScreenRectScaleBased(this RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            Vector3 position = transform.position;

            var rect = new Rect(position.x, Screen.height - position.y, size.x, size.y);

            Vector2 pivot = transform.pivot;
            rect.x -= pivot.x * size.x;
            rect.y -= (1.0f - pivot.y) * size.y;

            return rect;
        }
    }
}