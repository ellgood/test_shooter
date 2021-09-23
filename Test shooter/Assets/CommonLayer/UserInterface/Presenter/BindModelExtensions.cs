using System;
using UnityEngine.Assertions;

namespace CommonLayer.UserInterface.Presenter
{
    public static class BindModelExtensions
    {
        public static bool TryUseModel<TContract>(this PresenterBase presenter, TContract model)
        {
            Assert.IsNotNull(presenter);

            if (model == null || !(presenter is IModelContract<TContract> contract))
            {
                return false;
            }

            contract.UseModel(model);
            return true;
        }

        public static PresenterBase UseModel<TContract>(this PresenterBase presenter, TContract model)
        {
            Assert.IsNotNull(presenter);

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Model in contract can't be null");
            }

            if (presenter is IModelContract<TContract> contract)
            {
                contract.UseModel(model);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Can't use unimplemented contract <{typeof(TContract).Name}> in presenter {presenter}");
            }

            return presenter;
        }
    }
}