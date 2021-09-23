using System.Collections;
using CommonLayer.UserInterface.DataBinding;
using CommonLayer.UserInterface.Utility;
using CommonLayer.UserInterface.Utility.ExecutionOrder;
using CommonLayer.UserInterface.Views;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace CommonLayer.UserInterface.Binder.Collection
{
    [ExecuteBefore(typeof(ScrollRect))]
    public sealed class VerticalCollectionView : SimpleCollectionView
    {
        [SerializeField]
        private ScrollRect _scrollRect;

        [SerializeField]
        private RectOffset _padding;

        [SerializeField]
        private float _spacing;

        [SerializeField]
        private bool _scrollIfBordered = true;

        private readonly Deque<ViewWithIdx> _elements = new Deque<ViewWithIdx>();
 
        private RectTransform _content;

        private float _contentSize;
        private int _count;
        private bool _elementsIsDirty;
        private bool _forceRefresh;

        private float _itemHeight;
        private RectTransform _viewport;
        private float _viewportSize;

        private bool _visibleIsDirty;

        private IdxRange _visibleRange;

        public void SetDirty(bool force)
        {
            _visibleIsDirty = true;
            _elementsIsDirty = true;
            _forceRefresh = force;
        }

        protected override void Awake()
        {
            base.Awake();

            if (!_scrollRect)
            {
                _scrollRect = GetComponent<ScrollRect>();
            }
        }

        protected override void Start()
        {
            base.Start();

            Assert.IsNotNull(_scrollRect);

            InitSetup();
            StartCoroutine(DelayedRepaint());
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _scrollRect.onValueChanged.AddListener(OnScrollChanged);

            SetDirty(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _scrollRect.onValueChanged.RemoveListener(OnScrollChanged);

            for (var i = 0; i < _elements.Count; i++)
            {
                ReturnViewElement(_elements[i].View);
            }

            _elements.Clear();
        }

        protected override void OnItemAdded(int idx, BindContext bindContext)
        {
            _visibleIsDirty = true;
            if (_visibleRange.Contains(idx))
            {
                _forceRefresh = true;
            }
        }

        protected override void OnItemRemoved(int idx)
        {
            RemoveItem(idx);
        }

        protected override void OnItemReplaced(int idx, BindContext bindContext)
        {
            if (!_visibleRange.Contains(idx))
            {
                return;
            }

            int offset = idx - _visibleRange.From;
            ViewElement view = _elements[offset].View;
            view.DropContext();
            view.SetContext(bindContext);
        }

        protected override void OnClear()
        {
            _visibleIsDirty = true;
        }

        private void LateUpdate()
        {
            if (_visibleIsDirty)
            {
                RefreshContentSize(_scrollRect.verticalNormalizedPosition);
                if (RefreshVisibleRange(_scrollRect.verticalNormalizedPosition))
                {
                    _elementsIsDirty = true;
                }

                _visibleIsDirty = false;
            }

            if (!_elementsIsDirty && !_forceRefresh)
            {
                return;
            }

            RefreshElements(_forceRefresh);
            _forceRefresh = false;

            UpdatePositions();
            _elementsIsDirty = false;
        }

        private IEnumerator DelayedRepaint()
        {
            yield return new WaitForEndOfFrame();
            SetDirty(true);
        }

        private void RemoveItem(int idx)
        {
            _visibleIsDirty = true;
            if (_visibleRange.Contains(idx))
            {
                _forceRefresh = true;
            }
        }

        private IdxRange GetVisibleRange(float normPosition)
        {
            // The vertical scroll position as a value between 1 and 0, with 1 being at the bottom.
            float horizontalScroll = 1 - Mathf.Clamp01(normPosition);
            float topCursor = (_contentSize - _viewportSize) * horizontalScroll;
            float bottomCursor = topCursor + _viewportSize;

            return new IdxRange(GetItemIdx(topCursor) - 1, GetItemIdx(bottomCursor) + 1);
        }

        private float GetCursorPosition(int idx)
        {
            return _padding.top + idx * (_itemHeight + _spacing);
        }

        private int GetItemIdx(float cursorPosition)
        {
            return (int) ((cursorPosition - _padding.top) / (_itemHeight + _spacing));
        }

        private void InitSetup()
        {
            _content = _scrollRect.content;
            _viewport = _scrollRect.viewport;

            _itemHeight = _template.gameObject.GetComponent<RectTransform>().rect.height;
        }

        private void RefreshContentSize(float cursorPosition)
        {
            _count = Binder.Initialized ? Binder.Count : 0;
            //Calculate

            _contentSize = _count * _itemHeight + _spacing * (_count - 1) + _padding.vertical;

            _viewportSize = _viewport.GetSizeY();

            //Apply
            if (_scrollIfBordered && Mathf.Approximately(cursorPosition, 0) && _contentSize > _viewportSize)
            {
                _content.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, _contentSize);
            }
            else
            {
                _content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _contentSize);
            }
        }

        private void UpdatePositions()
        {
            for (var i = 0; i < _elements.Count; i++)
            {
                ViewWithIdx element = _elements[i];

                float cursor = GetCursorPosition(element.Idx);

                RectTransform rect = element.View.RectTransform;

                rect.SetSizeFromTop(_itemHeight);
                rect.offsetMin = new Vector2(_padding.left, -cursor - _itemHeight);
                rect.offsetMax = new Vector2(-_padding.right, -cursor);
            }
        }

        private bool RefreshVisibleRange(float cursorPositionNrm)
        {
            IdxRange newRange = IdxRange.Clamp(GetVisibleRange(cursorPositionNrm), 0, _count - 1);
            if (_visibleRange.Equals(newRange))
            {
                return false;
            }

            _visibleRange = newRange;
            return true;
        }

        private void RefreshElements(bool force = false)
        {
            int elementCount = _elements.Count;
            if (elementCount > 0 && force)
            {
                for (var i = 0; i < elementCount; i++)
                {
                    ViewElement template = _elements[i].View;
                    ReturnViewElement(template);
                }

                _elements.Clear();
                elementCount = 0;
            }

            if (elementCount == 0)
            {
                for (int i = _visibleRange.From; i <= _visibleRange.To; i++)
                {
                    ViewElement template = RentViewElement();
                    template.SetContext(Binder[i]);

                    var insert = new ViewWithIdx(i, template);
                    _elements.AddToRight(insert);
                }

#if COLLECTION_VIEW_DEBUG
                Debug.Log($"Fill empty [{_visibleRange.From}, {_visibleRange.To}]");
#endif
                return;
            }

            ViewWithIdx first = _elements[0];
            ViewWithIdx last = _elements[elementCount - 1];

            //Calculate total replace
            if (_visibleRange.To < first.Idx || _visibleRange.From > last.Idx)
            {
#if COLLECTION_VIEW_DEBUG
                Debug.Log($"Replace [{first.Idx}, {last.Idx}] to [{_visibleRange.From}, {_visibleRange.To}]");
#endif

                for (var i = 0; i < elementCount; i++)
                {
                    ViewElement template = _elements[i].View;
                    ReturnViewElement(template);
                }

                _elements.Clear();

                //TODO: Add new range
                for (int i = _visibleRange.From; i <= _visibleRange.To; i++)
                {
                    ViewElement template = RentViewElement();
                    template.SetContext(Binder[i]);

                    first = new ViewWithIdx(i, template);
                    _elements.AddToRight(first);
                }
            }
            else
            {
                //Check fix last element side
                if (_visibleRange.To != last.Idx)
                {
                    //if range.to > last.idx add range [last.idx+1, range.to]
                    //else range.to < last.idx remove range [range.to+1, last.idx]
                    if (last.Idx < _visibleRange.To)
                    {
#if COLLECTION_VIEW_DEBUG
                        Debug.Log($"Fix last Add [{last.Idx + 1}, {_visibleRange.To}]");
#endif
                        //TODO: Add range [first.idx, range.to]
                        while (last.Idx < _visibleRange.To && last.Idx < _count)
                        {
                            int idx = last.Idx + 1;

                            ViewElement template = RentViewElement();
                            template.SetContext(Binder[idx]);

                            last = new ViewWithIdx(idx, template);
                            _elements.AddToRight(last);
                        }
                    }
                    else
                    {
#if COLLECTION_VIEW_DEBUG
                        Debug.Log($"Fix last Remove [{_visibleRange.To + 1}, {last.Idx}]");
#endif
                        //TODO: Remove range [range.to+1, first.idx]
                        do
                        {
                            last = _elements.PopEnd();
                            ReturnViewElement(last.View);
                        }
                        while (last.Idx > _visibleRange.To + 1);
                    }
                }

                if (_visibleRange.From != first.Idx)
                {
                    //if range.From < last.idx add range [range.From, first.Idx-1]
                    //else range.From > last.idx remove range [first.Idx, range.From-1]
                    if (_visibleRange.From < first.Idx)
                    {
#if COLLECTION_VIEW_DEBUG
                        Debug.Log($"Fix first Add [{_visibleRange.From}, {first.Idx - 1}]");
#endif
                        while (_visibleRange.From < first.Idx && first.Idx > 0)
                        {
                            int idx = first.Idx - 1;

                            ViewElement template = RentViewElement();
                            template.SetContext(Binder[idx]);

                            first = new ViewWithIdx(idx, template);
                            _elements.AddBegin(first);
                        }
                    }
                    else
                    {
#if COLLECTION_VIEW_DEBUG
                        Debug.Log($"Fix first Remove [{first.Idx}, {_visibleRange.From - 1}]");
#endif
                        do
                        {
                            first = _elements.PopBegin();
                            ReturnViewElement(first.View);
                        }
                        while (first.Idx < _visibleRange.From - 1);
                    }
                }
            }
        }

        private void OnScrollChanged(Vector2 scroll)
        {
            _visibleIsDirty = true;
        }
    }
}