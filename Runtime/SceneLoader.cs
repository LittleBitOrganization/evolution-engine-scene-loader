using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LittleBit.Modules.CoreModule;
using LittleBit.Modules.SceneLoader.Description;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace LittleBit.Modules.SceneLoader
{
    public class SceneLoader : IService
    {
        public static event Action OnAllScenesLoaded; //TODO: provide a better way for this
        public event Action<float, string> OnUpdate;
        
        private readonly ICoroutineRunner _coroutineRunner;
        private ISceneLoaderService _sceneLoaderService;
        private IScenesConfig _scenesConfig;
        private List<CommandWithSceneName> _commandLoadLoadingScreen;
        private List<CommandWithSceneName> _commandLoadAllScenes;
        
        public class CommandWithSceneName
        {
            private LoadSceneCommand _loadSceneCommand;
            private string _nameScene = "";

            public CommandWithSceneName(LoadSceneCommand loadSceneCommand, string nameScene)
            {
                _loadSceneCommand = loadSceneCommand;
                _nameScene = nameScene;
            }

            public LoadSceneCommand LoadSceneCommand => _loadSceneCommand;

            public string NameScene => _nameScene;
        }
        
        public SceneLoader(ISceneLoaderService sceneLoaderService, ICoroutineRunner coroutineRunner,
            IScenesConfig scenesConfig)
        {
            _sceneLoaderService = sceneLoaderService;
            _scenesConfig = scenesConfig;

            InitCommands();

            _coroutineRunner = coroutineRunner;

            _coroutineRunner.StartCoroutine(LoadScenes());

            _sceneLoaderService.OnLoadScene += scene =>
            {
                if (scene.path == _scenesConfig.ActiveScene.SceneReference.ScenePath)
                {
                    SceneManager.SetActiveScene(scene);
                }
            };
        }

        private void InitCommands()
        {
            _commandLoadLoadingScreen = CreateCommands(LoadSceneRelationship.Child, _scenesConfig.LoadingScreenScene);
            _commandLoadAllScenes = CreateCommands(LoadSceneRelationship.Child, _scenesConfig.QueueScenes.ToArray());
        }

        private IEnumerator LoadScenes()
        {
            Queue<CommandWithSceneName> loadSceneCommands = new Queue<CommandWithSceneName>();
            var listCommands = new List<CommandWithSceneName>();

            listCommands.AddRange(_commandLoadLoadingScreen);
            listCommands.AddRange(_commandLoadAllScenes);

            foreach (var command in listCommands)
            {
                loadSceneCommands.Enqueue(command);
            }

            bool isDone = false;

            float currentProgress = 0;
            float maxProgress = loadSceneCommands.Count;
            float GetProgress(float current, float max) => current / max;

            float loadingStartTime = Time.time;


            while (!isDone)
            {
                bool isLoaded = false;

                if (loadSceneCommands.Count > 0)
                {
                    float progressLevel = 0;
                    var commandWithName = loadSceneCommands.Dequeue();

                    commandWithName.LoadSceneCommand.OnComplete += () =>
                    {
                        currentProgress += progressLevel;
                        isLoaded = true;
                    };

                    commandWithName.LoadSceneCommand.OnUpdateProgress += (value) =>
                    {
                        progressLevel = value;
                        OnUpdate?.Invoke(GetProgress(currentProgress + value, maxProgress), commandWithName.NameScene);
                    };

                    commandWithName.LoadSceneCommand.Load();
                }
                else
                {
                    var loadingEndTime = Time.time;
                    var loadingDuration = loadingEndTime - loadingStartTime;

                    var remainingTime = _scenesConfig.MinLogoDuration - loadingDuration;

                    OnAllScenesLoaded?.Invoke();

                    yield return new WaitForSeconds(Mathf.Max(0, remainingTime));

                    _commandLoadLoadingScreen.First().LoadSceneCommand.Unload(() => { });
                }

                yield return new WaitWhile(() => isLoaded == false);
            }
        }

        private void OnLoadFinish()
        {
            _commandLoadLoadingScreen.First().LoadSceneCommand.Unload(() => { });

            OnAllScenesLoaded?.Invoke();
        }
        
        private List<CommandWithSceneName> CreateCommands(
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