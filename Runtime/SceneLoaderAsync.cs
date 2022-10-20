using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LittleBit.Modules.CoreModule;
using LittleBit.Modules.SceneLoader.Description;
using UnityEngine.SceneManagement;
using Zenject;

namespace LittleBit.Modules.SceneLoader
{
    public class SceneLoaderAsync : SceneLoaderBase
    {

        private readonly ZenjectSceneLoader _zenjectSceneLoader;

        public SceneLoaderAsync(ISceneLoaderService sceneLoaderService,
            ZenjectSceneLoader zenjectSceneLoader) : base(sceneLoaderService)
        {

            _zenjectSceneLoader = zenjectSceneLoader;
        }
        
        
        public async Task LoadSceneAsync(CancellationToken token, SceneDescription scene)
        {
            var asyncOperation = _zenjectSceneLoader.LoadSceneAsync(scene.NameScene, LoadSceneMode.Additive, containerMode: LoadSceneRelationship.Child);
            asyncOperation.allowSceneActivation = false;
    
            while (true)
            {
                if (token.IsCancellationRequested) return;
                if (asyncOperation.progress >= 0.9f) break;
                await Task.Delay(50, token);
            }
            asyncOperation.allowSceneActivation = true;
        }
        
        public async Task LoadSceneAsync(CancellationToken token, params SceneDescription [] scenes)
        {
            Queue<SceneDescription> queue = new Queue<SceneDescription>(scenes);
            while (queue.Count > 0)
            {
                var scene = queue.Dequeue();
                await LoadSceneAsync(token, scene);
            }
        }

        
    }
}