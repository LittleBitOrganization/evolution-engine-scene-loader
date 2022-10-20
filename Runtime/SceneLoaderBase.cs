using System.Collections.Generic;
using LittleBit.Modules.SceneLoader.Description;
using Zenject;

namespace LittleBit.Modules.SceneLoader
{
    public class SceneLoaderBase
    {
        private readonly ISceneLoaderService _sceneLoaderService;

        public SceneLoaderBase(ISceneLoaderService sceneLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;
        }
        
        public List<CommandWithSceneName> CreateCommands(
            LoadSceneRelationship loadSceneRelationship = LoadSceneRelationship.None,
            params SceneDescription[] sceneDescriptions)
        {
            List<CommandWithSceneName> commands = new List<CommandWithSceneName>();
            foreach (var scenesReference in sceneDescriptions)
            {
                var loadSceneCommand = new LoadSceneCommand(_sceneLoaderService, scenesReference.SceneReference,
                    loadSceneRelationship);
                var nameScene = scenesReference.NameScene;

                commands.Add(new CommandWithSceneName(loadSceneCommand, nameScene));
            }

            return commands;
        }
    }
}