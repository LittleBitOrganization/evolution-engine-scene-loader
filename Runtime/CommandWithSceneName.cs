namespace LittleBit.Modules.SceneLoader
{
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
}