using System.Collections.Generic;

namespace LittleBit.Modules.SceneLoader.Description
{
    public interface IScenesConfig
    {
        public SceneDescription LoadingScreenScene { get; }
        public IReadOnlyList<SceneDescription> QueueScenes { get; }
        public float MinLogoDuration { get; }
        public SceneDescription ActiveScene { get; }
        public SceneDescription BootstrapScene { get; }
        public IReadOnlyList<SceneDescription> LocationScenes { get; }
    }
}