using System;
using Zenject;

namespace LittleBit.Modules.SceneLoader
{
    public class LoadSceneCommand : SceneLoaderCommand
    {
        public event Action OnComplete;
        public event Action<float> OnUpdateProgress;

        public LoadSceneCommand(ISceneLoaderService sceneLoaderService, SceneReference sceneReference, LoadSceneRelationship loadSceneRelationship = LoadSceneRelationship.None) : base(
            sceneLoaderService, sceneReference.ScenePath, loadSceneRelationship) { }
        
        public LoadSceneCommand(ISceneLoaderService sceneLoaderService, string pathScene, LoadSceneRelationship loadSceneRelationship = LoadSceneRelationship.None) : base(
            sceneLoaderService, pathScene, loadSceneRelationship) { }

        public override void Load()
        {
            SceneLoaderService.LoadSceneAsync(PathScene, OnUpdateProgressLoad, OnCompleteLoad, LoadSceneRelationship);
            
            void OnCompleteLoad() => OnComplete?.Invoke();
            void OnUpdateProgressLoad(float value) => OnUpdateProgress?.Invoke(value);
        }

        public override void Unload(Action onComplete)
        {
            SceneLoaderService.UnloadSceneAsync(PathScene, null, onComplete);

        }

    }
    
}