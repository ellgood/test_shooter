using System.Collections.Generic;
using TaskStatus = CommonLayer.SceneControllers.Routines.TaskStatus;


namespace CommonLayer.SceneControllers
{
    public interface ISceneControllerLoader
    {
        IEnumerator<TaskStatus> Load();

        IEnumerator<TaskStatus> Unload();
    }
}