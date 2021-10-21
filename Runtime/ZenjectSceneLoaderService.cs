using System;
using System.Collections;
using LittleBit.Modules.CoreModule;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace LittleBit.Modules.SceneLoader
{
    public class ZenjectSceneLoaderService : ISceneLoaderService
    {
        private readonly ZenjectSceneLoader _zenjectSceneLoader;
        private readonly ICoroutineRunner _coroutineRunner;
        
        public ZenjectSceneLoaderService(ZenjectSceneLoader zenjectSceneLoader, ICoroutineRunner coroutineRunner)
        {
            _zenjectSceneLoader = zenjectSceneLoader;
            _coroutineRunner = coroutineRunner;
        }

        public void LoadSceneAsync(string pathScene, Action<float> onProgressUpdate, Action onComplete, LoadSceneRelationship loadSceneRelationship = LoadSceneRelationship.None)
        {
            _coroutineRunner.StartCoroutine(LoadingSceneAsync(pathScene, onProgressUpdate, onComplete));
        }
        

        public void UnloadSceneAsync(string pathScene, Action<float> onProgressUpdate, Action onComplete)
        {
            throw new NotImplementedException();
        }
        
        private IEnumerator LoadingSceneAsync(string scene, Action<float> onProgressUpdate, Action onComplete, LoadSceneRelationship loadSceneRelationship = LoadSceneRelationship.None)
        {
            AsyncOperation asyncOperation =_zenjectSceneLoader.LoadSceneAsync(scene, LoadSceneMode.Additive, null, loadSceneRelationship);
            while (!asyncOperation.isDone)
            {
                onProgressUpdate?.Invoke(Mathf.Clamp(asyncOperation.progress / 0.9f, 0, 1));
                yield return null;
            }

            onComplete?.Invoke();
        }
    }
}
