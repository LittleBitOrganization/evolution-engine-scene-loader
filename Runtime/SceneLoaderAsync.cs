using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        
        private IEnumerator RoutineLoadSceneAsync(SceneDescription scene, Action<float> onUpdateProgress, Action onComplete)
        {
            var asyncOperation = _zenjectSceneLoader.LoadSceneAsync(scene.SceneReference.ScenePath, LoadSceneMode.Additive, null, containerMode: LoadSceneRelationship.Child);
            float progress = 0;
            asyncOperation.allowSceneActivation = false;
    
            while (!asyncOperation.isDone)
            {
                progress += Time.deltaTime;
                progress = Mathf.Min(progress, 1);
                onUpdateProgress?.Invoke(progress);
                
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
            onUpdateProgress?.Invoke(1);
            onComplete?.Invoke();
        }
        
        private IEnumerator RoutineLoadScenesAsync(List<SceneDescription> scenes, Action<float> onUpdateProgress, Action complete)
        {
            Queue<SceneDescription> queue = new Queue<SceneDescription>(scenes);

            float loadedScenes = 0;
            float GetProgress(float current, float max) => current / max;
            
            while (queue.Count > 0)
            {
                var scene = queue.Dequeue();
                yield return RoutineLoadSceneAsync(scene, (progress) =>
                {
                    float currentProgress = loadedScenes + progress;
                    onUpdateProgress?.Invoke(GetProgress(currentProgress, scenes.Count));
                }, null);
                loadedScenes++;
            }
            onUpdateProgress?.Invoke(GetProgress(loadedScenes, scenes.Count));
            complete?.Invoke();
        }


        public async Task LoadSceneAsync(CancellationToken token, Action<float> onUpdateProgress, SceneDescription scene)
        {
            bool isDone = false;
            _coroutineRunner.StartCoroutine(RoutineLoadSceneAsync(scene, onUpdateProgress, () => isDone = true));
            while (isDone == false)
            {
                await Task.Delay(50, token);
            }
        }
        
        public async Task LoadSceneAsync(CancellationToken token, Action<float> onUpdateProgress, SceneDescription [] scenes)
        {
            bool isDone = false;
            _coroutineRunner.StartCoroutine(RoutineLoadScenesAsync(scenes.ToList(), onUpdateProgress, () => isDone = true));
          
            while (isDone == false)
            {
                await Task.Delay(50, token);
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