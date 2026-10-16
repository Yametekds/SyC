using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneFromGameplay : ChangeScene, IGameHubDependecy 
{
    private ILevelLoaderController levelLoader;

    private void OnDestroy()
    {
        this.levelLoader.OnUnLoadComplete -= ChangeSceneMethod;
    }

    public void SetInjection(IGameHubComponentController injection)
    {
        injection.OnLoadComplete += this.OnHubCompleteLoad;
    }

    private void OnHubCompleteLoad(IGameHubComponentController hub)
    {
        hub.OnLoadComplete -= this.OnHubCompleteLoad;
        SetLevelLoadController(hub.GetLevelLoaderController());
    }

    private void SetLevelLoadController(ILevelLoaderController injection)
    {
        this.levelLoader = injection;
        this.levelLoader.OnUnLoadComplete += ChangeSceneMethod;
    }

    
}
