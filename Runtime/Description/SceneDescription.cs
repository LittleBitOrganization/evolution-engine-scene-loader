using UnityEngine;

namespace LittleBit.Modules.SceneLoader.Description
{
    [CreateAssetMenu(fileName = "DescriptionScene", menuName = "Configs/Scenes/DescriptionScene", order = 0)]
    public class SceneDescription : ScriptableObject
    {
        [SerializeField] private string nameScene;
        [SerializeField] private SceneReference sceneReference;

        public string NameScene => nameScene;
        public SceneReference SceneReference => sceneReference;
    }
}