using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace LittleBit.Modules.SceneLoader
{
    public abstract class SceneLoaderCommand
    {
        protected ISceneLoaderService SceneLoaderService;
        protected string PathScene;
        protected LoadSceneRelationship LoadSceneRelationship = LoadSceneRelationship.None;

        protected Scene SceneData;
        public Scene Data => SceneData;

        protected GameObject RootGameObject;

        protected SceneLoaderCommand(ISceneLoaderService sceneLoaderService, string pathScene, LoadSceneRelationship loadSceneRelationship = LoadSceneRelationship.None)
        {
            SceneLoaderService = sceneLoaderService;
            PathScene = pathScene;
            SceneData = SceneManager.GetSceneByPath(PathScene);
        }

        public abstract void Load();
        public abstract void Unload(Action onComplete);
    }
    
    
}