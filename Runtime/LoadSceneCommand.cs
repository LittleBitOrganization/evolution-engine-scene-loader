using System;
using UnityEngine.SceneManagement;

namespace LittleBit.Modules.SceneLoader
{
    public class LoadSceneCommand : SceneLoaderCommand
    {
        public event Action OnComplete;
        public event Action<float> OnUpdateProgress;

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

        public override void Load()
        {
            SceneLoaderService.LoadSceneAsync(PathScene, OnUpdateProgressLoad, OnCompleteLoad);
            
            void OnCompleteLoad() => OnComplete?.Invoke();
            void OnUpdateProgressLoad(float value) => OnUpdateProgress?.Invoke(value);
        }

        public override void Unload(Action onComplete)
        {
            SceneLoaderService.UnloadScene(PathScene, null, onComplete);

        }

    }
    
}