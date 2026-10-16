
using UnityEngine;
using UnityEngine.UI;

namespace MainMenuResourcesLoader
{
    public class UIController : LoadingBarController
    {

        public MainMenuLoaderResources menuLoader;
        public ChangeScene sceneLoader;


        public override void Subscriptions()
        {
            menuLoader.OnLoadUpdate.AddListener(UpdateScroll);
            sceneLoader.OnSceneLoadUpdate.AddListener(UpdateScroll);
        }

        private void Awake()
        {
            Subscriptions();
        }

        
    }
}