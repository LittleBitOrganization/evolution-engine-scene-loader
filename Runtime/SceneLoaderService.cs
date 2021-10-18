using System;
using System.Collections;
using LittleBit.Modules.CoreModule;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LittleBit.Modules.SceneLoader
{

    public class SceneLoaderService : IService
    {
        private ICoroutineRunner _coroutineRunner;

        public SceneLoaderService(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void LoadSceneAsync(string pathScene, Action<float> onProgressUpdate, Action onComplete)
        {
            _coroutineRunner.StartCoroutine(LoadingSceneAsync(pathScene, onProgressUpdate, onComplete));
        }

        public void UnloadScene(string pathScene, Action<float> onProgressUpdate, Action onComplete)
        {
            _coroutineRunner.StartCoroutine(UnloadSceneAsync(pathScene, onComplete));
        }

        private IEnumerator LoadingSceneAsync(string scene, Action<float> onProgressUpdate, Action onComplete)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            while (!asyncOperation.isDone)
            {
                onProgressUpdate?.Invoke(Mathf.Clamp(asyncOperation.progress / 0.9f, 0, 1));
                yield return null;
            }

            onComplete?.Invoke();
        }

        private IEnumerator UnloadSceneAsync(string scene, Action onComplete)
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
