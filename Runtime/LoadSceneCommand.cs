using System;
using UnityEngine.SceneManagement;

namespace LittleBit.Modules.SceneLoader
{
    public class LoadSceneCommand : SceneLoaderCommand
    {
        private Action _onComplete;
        
        public LoadSceneCommand(SceneLoaderService sceneLoaderService, SceneReference sceneReference) : base(
            sceneLoaderService, sceneReference.ScenePath)
        {
            SceneData = SceneManager.GetSceneByPath(PathScene);
        }
        
        public LoadSceneCommand(SceneLoaderService sceneLoaderService, string pathScene) : base(
            sceneLoaderService, pathScene)
        {
            SceneData = SceneManager.GetSceneByPath(PathScene);
        }

        public override void Load(Action onComplete)
        {
            SceneLoaderService.LoadSceneAsync(PathScene, null, onComplete);
        }

        public override void Unload(Action onComplete)
        {
            SceneLoaderService.UnloadScene(PathScene, null, onComplete);

        }

    }
    
}