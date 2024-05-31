using Managers;

namespace InteractableObjects
{
    public class SceneTransitionObject : BasicObject
    {
        public GameSceneManager gameSceneManager;
        public string toSceneName;

        protected override void Initialize()
        {
            gameSceneManager = FindObjectOfType<GameSceneManager>();
        }

        public override bool Interact()
        {
            gameSceneManager.SwitchToScene(toSceneName);
            return true;
        }
    }
}