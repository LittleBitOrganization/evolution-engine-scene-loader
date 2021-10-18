using UnityEngine.SceneManagement;

namespace LittleBit.Modules.SceneLoader
{
    public class LoadSceneCommand : SceneLoaderCommand
    {
        public LoadSceneCommand(SceneLoaderService sceneLoaderService, SceneReference sceneReference) : base(
            sceneLoaderService, sceneReference.ScenePath)
        {
            SceneData = SceneManager.GetSceneByPath(PathScene);
        }
        
        public LoadSceneCommand(SceneLoaderService sceneLoaderService, string pathScene) : base(
            sceneLoaderService, pathScene)
        {
            SceneData = SceneManager.GetSceneByPath(PathScene);
        }

        public override void Load()
        {
            SceneLoaderService.LoadSceneAsync(PathScene, null, OnUnload);
        }

        public override void Unload()
        {
            SceneLoaderService.UnloadScene(PathScene, null, null);
        }

        private void OnUnload()
        {

        }
    }
}