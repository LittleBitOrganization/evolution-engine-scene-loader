using System;
using UnityEngine.SceneManagement;
using Zenject;

namespace LittleBit.Modules.SceneLoader
{
    public interface ISceneLoaderService : IService
    {
        public void LoadSceneAsync(string pathScene, Action<float> onProgressUpdate, Action onComplete, LoadSceneRelationship loadSceneRelationship = LoadSceneRelationship.None);
        public void UnloadSceneAsync(Scene scene, Action<float> onProgressUpdate, Action onComplete);
    }
}