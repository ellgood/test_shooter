using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace CommonLayer.UserInterface.Controls
{
    [RequireComponent(typeof(RectTransform))]
    public class WindowControl : UIBehaviour
    {
        [SerializeField]
        private UIBehaviour _dragHandler;

        [SerializeField]
        private bool _draggable = true;

        private readonly CompositeDisposable _dragSubscribes = new CompositeDisposable();
        private Vector2 _currentDrag;

        private bool _isDragging;

        private Vector2 _startDrag;

        public bool IsDraggable => _draggable && _dragHandler != null && _dragHandler;

        public RectTransform RectTransform { get; private set; }

        public Vector2 WindowPosition { get; set; }

        protected override void Awake()
        {
            var targetTransform = GetComponent<RectTransform>();
            Assert.IsNotNull(targetTransform);
            Assert.IsTrue(targetTransform);

            RectTransform = targetTransform;
        }

        private void Update()
        {
            if (_isDragging)
            {
                RectTransform.anchoredPosition = WindowPosition + _currentDrag;
            }
            else
            {
                RectTransform.anchoredPosition = WindowPosition;
            }
        }

        protected override void OnEnable()
        {
            WindowPosition = RectTransform.anchoredPosition;

            if (_dragHandler != null && _dragHandler)
            {
                _dragHandler.OnBeginDragAsObservable().Subscribe(OnBeginDrag).AddTo(_dragSubscribes);
                _dragHandler.OnDragAsObservable().Subscribe(OnDrag).AddTo(_dragSubscribes);
                _dragHandler.OnEndDragAsObservable().Subscribe(OnEndDrag).AddTo(_dragSubscribes);
            }
        }

        protected override void OnDisable()
        {
            WindowPosition = Vector2.zero;

            _dragSubscribes.Clear();
        }

        public void SetDraggable(bool draggable)
        {
            _draggable = draggable;
        }

        private void OnEndDrag(PointerEventData eventData)
        {
            Vector2 dragPos = eventData.position / RectTransform.lossyScale;

            _currentDrag = dragPos - _startDrag;
            WindowPosition += _currentDrag;

            _isDragging = false;
        }

        private void OnDrag(PointerEventData eventData)
        {
            Vector2 dragPos = eventData.position / RectTransform.lossyScale;

            _currentDrag = dragPos - _startDrag;
        }

        private void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            _startDrag = eventData.position / RectTransform.lossyScale;
        }
    }
}