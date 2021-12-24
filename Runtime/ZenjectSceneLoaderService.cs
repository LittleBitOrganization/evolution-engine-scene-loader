using System;
using System.Collections;
using LittleBit.Modules.CoreModule;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using Zenject;
using Object = UnityEngine.Object;

namespace LittleBit.Modules.SceneLoader
{
    public class ZenjectSceneLoaderService : ISceneLoaderService
    {
        private readonly ZenjectSceneLoader _zenjectSceneLoader;
        private readonly ICoroutineRunner _coroutineRunner;
        
        public event Action<Scene> OnLoadScene;
        
        [Preserve]
        public ZenjectSceneLoaderService(ZenjectSceneLoader zenjectSceneLoader, ICoroutineRunner coroutineRunner)
        {
            _zenjectSceneLoader = zenjectSceneLoader;
            _coroutineRunner = coroutineRunner;
        }

        public void LoadSceneAsync(string pathScene, Action<float> onProgressUpdate, Action onComplete, LoadSceneRelationship loadSceneRelationship = LoadSceneRelationship.None)
        {
            _coroutineRunner.StartCoroutine(LoadingSceneAsync(pathScene, onProgressUpdate, onComplete));
        }

        public void UnloadSceneAsync(Scene scene, Action<float> onProgressUpdate, Action onComplete)
        {
            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                Object.Destroy(rootGameObject);
            }
        }

        private IEnumerator LoadingSceneAsync(string scene, Action<float> onProgressUpdate, Action onComplete, LoadSceneRelationship loadSceneRelationship = LoadSceneRelationship.None)
        {
            AsyncOperation asyncOperation =_zenjectSceneLoader.LoadSceneAsync(scene, LoadSceneMode.Additive, null, loadSceneRelationship);
            while (!asyncOperation.isDone)
            {
                onProgressUpdate?.Invoke(Mathf.Clamp(asyncOperation.progress / 0.9f, 0, 1));
                yield return null;
            }

            OnLoadScene?.Invoke(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
            onComplete?.Invoke();
        }

      
    }
}
