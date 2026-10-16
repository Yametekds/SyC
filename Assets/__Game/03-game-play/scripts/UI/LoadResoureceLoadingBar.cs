using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadResoureceLoadingBar : LoadingBarController, IGameHubDependecy
{
    private ILevelLoaderController levelLoader;

    public void SetInjection(IGameHubComponentController injection)
    {
        injection.OnLoadComplete += this.OnHubCompleteLoad;
    }

    private void OnHubCompleteLoad(IGameHubComponentController hub)
    {
        hub.OnLoadComplete -= this.OnHubCompleteLoad;
        SetLevelLoadController(hub.GetLevelLoaderController());
    }

    public override void Subscriptions()
    {
        levelLoader.OnLoadUpdate += UpdateScroll;
    }

    private void SetLevelLoadController(ILevelLoaderController injection)
    {
        this.levelLoader = injection;
        Subscriptions();
    }

    private void OnDestroy()
    {
        levelLoader.OnLoadUpdate -= UpdateScroll;
    }
}
