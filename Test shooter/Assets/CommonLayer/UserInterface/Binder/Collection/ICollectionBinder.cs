using System;
using System.Collections.Generic;
using CommonLayer.UserInterface.Binder.Collection.Delegates;
using CommonLayer.UserInterface.DataBinding;

namespace CommonLayer.UserInterface.Binder.Collection
{
    public interface ICollectionBinder : IReadOnlyList<BindContext>
    {
        event CollectionChangeIdxAction ItemAdded;
        event CollectionRemoveAction ItemRemoved;
        event CollectionChangeIdxAction ItemReplaced;
        event Action Clear;

        bool Initialized { get; }
    }
}