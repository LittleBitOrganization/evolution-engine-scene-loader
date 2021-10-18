using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LittleBit.Modules.SceneLoader
{
    public abstract class SceneLoaderCommand
    {
        protected SceneLoaderService SceneLoaderService;
        protected string PathScene;

        protected Scene SceneData;
        public Scene Data => SceneData;

        protected GameObject RootGameObject;

        protected SceneLoaderCommand(SceneLoaderService sceneLoaderService, string pathScene)
        {
            SceneLoaderService = sceneLoaderService;
            PathScene = pathScene;
        }

        public abstract void Load(Action onComplete);
        public abstract void Unload(Action onComplete);
    }
    
    
}