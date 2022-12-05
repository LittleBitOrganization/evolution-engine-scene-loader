using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LittleBit.Modules.CoreModule;
using LittleBit.Modules.SceneLoader.Description;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace LittleBit.Modules.SceneLoader
{
    public class SceneLoaderAsync : SceneLoaderBase
    {

        private readonly ZenjectSceneLoader _zenjectSceneLoader;
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoaderAsync(ISceneLoaderService sceneLoaderService,
            ZenjectSceneLoader zenjectSceneLoader, ICoroutineRunner coroutineRunner) : base(sceneLoaderService)
        {
            _zenjectSceneLoader = zenjectSceneLoader;
            _coroutineRunner = coroutineRunner;
        }
        
        
      

        // public async Task RoutineLoadSceneAsync(Action<float> onUpdateProgress, SceneDescription scene)
        // {
        //     var asyncOperation = _zenjectSceneLoader.RoutineLoadSceneAsync(scene.SceneReference.ScenePath, LoadSceneMode.Additive, containerMode: LoadSceneRelationship.Child);
        //     asyncOperation.allowSceneActivation = false;
        //     
        //     while (!asyncOperation.isDone){
        //     
        //         Debug.LogError("AsyncProgressScene " + scene.NameScene + ": " + asyncOperation.progress);
        //         onUpdateProgress?.Invoke(asyncOperation.progress);
        //         
        //         if (token.IsCancellationRequested) return;
        //         
        //         if (asyncOperation.progress >= 0.9f)
        //         {
        //             asyncOperation.allowSceneActivation = true;
        //         }
        //         await Task.Delay(50, token);
        //     }
        // }

        
        private IEnumerator RoutineLoadSceneAsync(SceneDescription scene, Action<float> onUpdateProgress, Action complete)
        {
            var asyncOperation = _zenjectSceneLoader.LoadSceneAsync(scene.SceneReference.ScenePath, LoadSceneMode.Additive, containerMode: LoadSceneRelationship.Child);
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                Debug.LogError("AsyncProgressScene " + scene.NameScene + ": " + asyncOperation.progress);
                onUpdateProgress?.Invoke(asyncOperation.progress);

                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
            complete?.Invoke();
            
        }

        private async Task LoadSceneAsync(CancellationToken token, SceneDescription scene, Action<float> onUpdateProgress)
        {
            if (token.IsCancellationRequested) return;
            bool isDone = false;

            _coroutineRunner.StartCoroutine(RoutineLoadSceneAsync(scene, onUpdateProgress, () => isDone = true));
            while (isDone == false)
            {
                await Task.Delay(50, token);
            }
        }
        
        public async Task LoadSceneAsync(CancellationToken token, Action<float> onUpdateProgress, params SceneDescription [] scenes)
        {
            Queue<SceneDescription> queue = new Queue<SceneDescription>(scenes);
            
            float loadedScenes = 0;
            float GetProgress(float current, float max) => current / max;
            
            while (queue.Count > 0)
            {
                if (token.IsCancellationRequested) return;
                var scene = queue.Dequeue();
                
                await LoadSceneAsync(token, scene, (progress) =>
                {
                    float currentProgress = loadedScenes + progress;
                    onUpdateProgress?.Invoke(GetProgress(currentProgress, scenes.Length));
                });
                loadedScenes++;
                onUpdateProgress?.Invoke(GetProgress(loadedScenes, scenes.Length));
            }
        }
        
        
        public async Task UnloadSceneAsync(string scene)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(scene);
            while (!asyncOperation.isDone)
            {
                await Task.Delay(10);
            }
        }

        public async Task UnloadAllScenesAsync()
        {
            while (SceneManager.sceneCount > 1)
            {
                int index = SceneManager.sceneCount - 1;
                var currentScene = SceneManager.GetSceneAt(index);
                await UnloadSceneAsync(currentScene.name);
            }
        }

        
    }
}