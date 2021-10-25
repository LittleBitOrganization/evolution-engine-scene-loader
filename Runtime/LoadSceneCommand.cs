using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace LittleBit.Modules.SceneLoader
{
    public class LoadSceneCommand : SceneLoaderCommand
    {
        public event Action OnComplete;
        public event Action<Scene> OnCompleteLoad;
        public event Action<float> OnUpdateProgress;

        private Scene _scene;

        public LoadSceneCommand(ISceneLoaderService sceneLoaderService, SceneReference sceneReference, LoadSceneRelationship loadSceneRelationship = LoadSceneRelationship.None) : base(
            sceneLoaderService, sceneReference.ScenePath, loadSceneRelationship) { }
        
        public LoadSceneCommand(ISceneLoaderService sceneLoaderService, string pathScene, LoadSceneRelationship loadSceneRelationship = LoadSceneRelationship.None) : base(
            sceneLoaderService, pathScene, loadSceneRelationship) { }

        public override void Load()
        {
            SceneLoaderService.LoadSceneAsync(PathScene, OnUpdateProgressLoad, OnLoad, LoadSceneRelationship);
          
            void OnLoad()
            {
                _scene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
                OnComplete?.Invoke();
                OnCompleteLoad?.Invoke(_scene);
            }

            void OnUpdateProgressLoad(float value) => OnUpdateProgress?.Invoke(value);
        }

        public override void Unload(Action onComplete)
        {
            SceneLoaderService.UnloadSceneAsync(_scene, null, onComplete);

        }

    }
    
}