#if UNITY_EDITOR
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LittleBit.Modules.SceneLoader.Editor
{
    public class EditorSceneLoader
    {
        public static async Task UnloadSceneAsync(int indexInSceneManager)
        {
            Scene scene = EditorSceneManager.GetSceneAt(indexInSceneManager);
            await UnloadSceneAsync(scene);
        }

        public static async Task UnloadSceneAsync(string nameScene)
        {
            Scene scene = EditorSceneManager.GetSceneByName(nameScene);
            await UnloadSceneAsync(scene);
        }

        public static async Task UnloadSceneAsync(Scene scene)
        {
            AsyncOperation asyncOperation = EditorSceneManager.UnloadSceneAsync(scene);
            while (!asyncOperation.isDone)
            {
                await Task.Delay(10);
            }
        }
        
        public static Scene OpenScene(string scenePath, OpenSceneMode loadSceneMode = OpenSceneMode.Single)
        {
            return EditorSceneManager.OpenScene(scenePath, loadSceneMode);
        }

        public static IList<Scene> GetAllOpenScenes()
        {
            IList<Scene> scenes = new List<Scene>();
            for (int i = 0; i < EditorSceneManager.sceneCount; i++)
            {
                scenes.Add(EditorSceneManager.GetSceneAt(i));
            }

            return scenes;
        }

        public static Scene FirstScene => EditorSceneManager.GetSceneAt(0);
    }
}
#endif