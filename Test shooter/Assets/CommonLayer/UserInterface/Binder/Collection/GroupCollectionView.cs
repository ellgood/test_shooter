using CommonLayer.UserInterface.DataBinding;
using CommonLayer.UserInterface.Pooling;
using CommonLayer.UserInterface.Views;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder.Collection
{
    public sealed class GroupCollectionView : SimpleCollectionView
    {
        [SerializeField]
        private CollectionTemplateInfo _firstTemplateInfo;

        [SerializeField]
        private CollectionTemplateInfo _lastTemplateInfo;

        [SerializeField]
        private CollectionTemplateInfo _singleTemplateInfo;

        private UnityComponentPool<ViewElement> _firstViewPool;
        private UnityComponentPool<ViewElement> _lastViewPool;
        private UnityComponentPool<ViewElement> _singleViewPool;

        protected override void Awake()
        {
            base.Awake();

            Transform t = transform;

            _firstViewPool = new UnityComponentPool<ViewElement>(_firstTemplateInfo.Template,
                _firstTemplateInfo.TemplateContentPanel ? _firstTemplateInfo.TemplateContentPanel : t,
                _firstTemplateInfo.TemplatePool ? _firstTemplateInfo.TemplatePool : t);

            _lastViewPool = new UnityComponentPool<ViewElement>(_lastTemplateInfo.Template,
                _lastTemplateInfo.TemplateContentPanel ? _lastTemplateInfo.TemplateContentPanel : t,
                _lastTemplateInfo.TemplatePool ? _lastTemplateInfo.TemplatePool : t);

            _singleViewPool = new UnityComponentPool<ViewElement>(_singleTemplateInfo.Template,
                _singleTemplateInfo.TemplateContentPanel ? _singleTemplateInfo.TemplateContentPanel : t,
                _singleTemplateInfo.TemplatePool ? _singleTemplateInfo.TemplatePool : t);
        }

        protected override void OnItemAdded(int idx, BindContext bindContext)
        {
            int lastIdx = idx;
            int givenAmount = lastIdx + 1;

            bool idxIsFirst = idx == 0;
            bool idxIsSingle = idxIsFirst && givenAmount == 1;
            bool idxIsLast = idx == lastIdx;

            if (idxIsSingle)
            {
                ActivateTemplate(_singleViewPool, idx, bindContext);
            }
            else
            {
                if (idxIsFirst)
                {
                    ActivateTemplate(_firstViewPool, idx, bindContext);
                    int nextIdx = idx + 1;
                    if (nextIdx == lastIdx)
                    {
                        DeactivateTemplate(_singleViewPool, nextIdx);
                        ActivateTemplate(_firstViewPool, nextIdx, Binder[nextIdx]);
                    }
                    else
                    {
                        DeactivateTemplate(_firstViewPool, nextIdx);
                        base.OnItemAdded(nextIdx, Binder[nextIdx]);
                    }
                }
                else if (idxIsLast)
                {
                    int previousIdx = idx - 1;

                    if (previousIdx == 0)
                    {
                        DeactivateTemplate(_singleViewPool, previousIdx);
                        ActivateTemplate(_firstViewPool, previousIdx, Binder[previousIdx]);
                    }
                    else
                    {
                        DeactivateTemplate(_lastViewPool, previousIdx);
                        base.OnItemAdded(previousIdx, Binder[previousIdx]);
                    }

                    ActivateTemplate(_lastViewPool, idx, bindContext);
                }
                else
                {
                    base.OnItemAdded(idx, Binder[idx]);
                }
            }
        }

        protected override void OnItemRemoved(int idx)
        {
            int remainingAmount = Binder.Count;
            bool idxIsFirst = idx == 0;
            bool idxIsSingle = idxIsFirst && remainingAmount == 0;
            int lastIdx = remainingAmount;
            bool idxIsLast = idx == lastIdx;

            if (idxIsSingle)
            {
                DeactivateTemplate(_singleViewPool, idx);
            }
            else
            {
                if (idxIsFirst)
                {
                    DeactivateTemplate(_firstViewPool, idx);

                    if (remainingAmount == 1)
                    {
                        DeactivateTemplate(_lastViewPool, idx);
                        ActivateTemplate(_singleViewPool, idx, Binder[idx]);
                    }
                    else
                    {
                        base.OnItemRemoved(idx);
                        ActivateTemplate(_firstViewPool, idx, Binder[idx]);
                    }
                }
                else if (idxIsLast)
                {
                    DeactivateTemplate(_lastViewPool, idx);
                    int previousIdx = idx - 1;
                    if (previousIdx == 0)
                    {
                        DeactivateTemplate(_firstViewPool, previousIdx);
                        ActivateTemplate(_singleViewPool, previousIdx, Binder[previousIdx]);
                    }
                    else
                    {
                        base.OnItemRemoved(previousIdx);
                        ActivateTemplate(_lastViewPool, previousIdx, Binder[previousIdx]);
                    }
                }
                else
                {
                    base.OnItemRemoved(idx);
                }
            }
        }

        protected override void OnClear()
        {
            int givenAmount = Elements.Count;
            if (givenAmount == 0)
            {
                return;
            }

            if (givenAmount == 1)
            {
                DeactivateTemplate(_singleViewPool, 0);
                return;
            }

            DeactivateTemplate(_firstViewPool, 0);
            givenAmount--;
            int lastIdx = givenAmount - 1;
            DeactivateTemplate(_lastViewPool, lastIdx);

            if (lastIdx > 0)
            {
                base.OnClear();
            }
        }

        private void ActivateTemplate(UnityComponentPool<ViewElement> poolFrom, int idxTo, BindContext context)
        {
            ViewElement template = poolFrom.Rent();
            template.SetContext(context);
            Elements.Insert(idxTo, template);
            template.RectTransform.SetSiblingIndex(idxTo);
        }

        private void DeactivateTemplate(UnityComponentPool<ViewElement> poolTo, int idxFrom)
        {
            ViewElement template = Elements[idxFrom];
            Elements.RemoveAt(idxFrom);
            template.ResetView();
            poolTo.Return(template);
        }
    }
}