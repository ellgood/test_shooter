using UniRx;

namespace CommonLayer.UserInterface.DataBinding.Binds.Collections
{
    internal interface ICollectionRestrictedBind<TValue> : ICollectionBind
    {
        void ScopeInsert(CollectionAddEvent<TValue> collectionAddEvent);
        void ScopeRemove(CollectionRemoveEvent<TValue> collectionRemoveEvent);
        void ScopeReplace(CollectionReplaceEvent<TValue> collectionReplaceEvent);
        void ScopeMove(CollectionMoveEvent<TValue> collectionMoveEvent);
        void ScopeReset();
    }
}