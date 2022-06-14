using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LittleBit.Modules.SceneLoader.Description;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LittleBit.Modules.SceneLoader.Editor
{
    public class ToolSceneLoader
    {
        private const string PathConfigScenes = "Configs/Scenes Configs/Main/ContainerScenes";
        public static IScenesConfig ScenesConfig => LoadScenesConfig(PathConfigScenes);
        public static IScenesConfig LoadScenesConfig(string path)
        {
            try
            {
                return (IScenesConfig) Resources.Load(path);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\nСouldn't find or download " + path);
            }
        }
        
        private const int SceneLoader = 0;
        
        [MenuItem("Tools/Scene Loader/Open Bootstrap", false, SceneLoader)]
        public static void OpenBootstrap()
        {
            EditorSceneLoader.OpenScene(ScenesConfig.BootstrapScene.SceneReference.ScenePath, OpenSceneMode.Single);
        }
        
        [MenuItem("Tools/Scene Loader/Load Locations And UI &L", false)]
        public static async void LoadLocationsAndUI()
        {
            OpenBootstrap();
            IScenesConfig scenesConfig = ScenesConfig;
            
            if (scenesConfig.LocationScenes.Count == 0)
            {
                throw new Exception("Failed to upload. There are no scenes in the list");
            }

            List<SceneDescription> sceneDescriptions = new List<SceneDescription>();
            
            sceneDescriptions.Add(scenesConfig.BootstrapScene);
            sceneDescriptions.AddRange(scenesConfig.QueueScenes);
         
            
            foreach (var sceneDescription in sceneDescriptions)
            {
                await Task.Delay(25);
                EditorSceneLoader.OpenScene(sceneDescription.SceneReference.ScenePath, OpenSceneMode.Additive);
            }
        }
        
        [MenuItem("Tools/Scene Loader/Unload All Scenes &U", false, SceneLoader)]
        public static async Task UnloadAllScenes()
        {
            while (EditorSceneManager.loadedSceneCount > 1)
            {
                int index = EditorSceneManager.loadedSceneCount - 1;
                await EditorSceneLoader.UnloadSceneAsync(index);
            }
        }
    }
}