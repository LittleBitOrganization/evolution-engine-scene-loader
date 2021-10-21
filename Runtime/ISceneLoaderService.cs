using System;
using Zenject;

namespace LittleBit.Modules.SceneLoader
{
    public interface ISceneLoaderService : IService
    {
        public void LoadSceneAsync(string pathScene, Action<float> onProgressUpdate, Action onComplete, LoadSceneRelationship loadSceneRelationship = LoadSceneRelationship.None);
        public void UnloadSceneAsync(string pathScene, Action<float> onProgressUpdate, Action onComplete);
    }
}