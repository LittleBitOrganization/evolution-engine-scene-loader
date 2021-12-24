using System;
using System.Collections;
using LittleBit.Modules.CoreModule;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using Zenject;

namespace LittleBit.Modules.SceneLoader
{

    public class SceneLoaderService : ISceneLoaderService
    {
        private ICoroutineRunner _coroutineRunner;

        public event Action<Scene> OnLoadScene;
        public event Action<Scene> OnUnloadScene;
        
        [Preserve]
        public SceneLoaderService(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }
        
        public void LoadSceneAsync(string pathScene, Action<float> onProgressUpdate, Action onComplete,
            LoadSceneRelationship loadSceneRelationship = LoadSceneRelationship.None)
        {
            _coroutineRunner.StartCoroutine(LoadingSceneAsync(pathScene, onProgressUpdate, onComplete));
        }

        public void UnloadSceneAsync(Scene scene, Action<float> onProgressUpdate, Action onComplete)
        {
            _coroutineRunner.StartCoroutine(UnloadSceneAsync(scene, onComplete));
        }

        private IEnumerator LoadingSceneAsync(string scene, Action<float> onProgressUpdate, Action onComplete)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            while (!asyncOperation.isDone)
            {
                onProgressUpdate?.Invoke(Mathf.Clamp(asyncOperation.progress / 0.9f, 0, 1));
                yield return null;
            }

            OnLoadScene?.Invoke(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
            onComplete?.Invoke();
        }

        private IEnumerator UnloadSceneAsync(Scene scene, Action onComplete)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(scene);
            while (!asyncUnload.isDone)
            {
                yield return null;
            }

            onComplete?.Invoke();
        }
    }
}
