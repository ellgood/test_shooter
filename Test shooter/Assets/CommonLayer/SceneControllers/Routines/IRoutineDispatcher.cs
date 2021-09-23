using System.Collections.Generic;

namespace CommonLayer.SceneControllers.Routines
{
    public interface IRoutineDispatcher
    {
        bool HaveActiveRoutines { get; }

        void EnqueueRoutine(IEnumerator<TaskStatus> routine);
    }
}