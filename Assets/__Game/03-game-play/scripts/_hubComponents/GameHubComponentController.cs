using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameHubComponentController : CommonHubComponentController, IGameHubComponentController,
    ILevelLoaderDependency, IGameControllerDependecy, ITimeControllerDependecy, IBeatControllerDependency
{
    public event Action<IGameHubComponentController> OnLoadComplete;
    private static GameHubComponentController INSTANCE;

    private sealed class KeyArrayPosition {
        public const int LEVEL_LOADER_CONTROLLER = 0;
        public const int GAME_CONTROLLER = 1;
        public const int TIME_CONTROLLER = 2;
        public const int BEAT_CONTROLLER = 3;
    }


    private ILevelLoaderController levelLoaderController;
    private IGameController gameController;
    private ITimeController timeController;
    private IBeatController beatController;
    
    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(this);
        }
        else
        {
            INSTANCE = this;
        }
    }

    protected override void OnLoadCompleted()
    {
        this.OnLoadComplete?.Invoke(this);
        this.levelLoaderController.LoadResources();
    }

    protected override void InitialSetupComponents()
    {
        SetComponent(KeyArrayPosition.LEVEL_LOADER_CONTROLLER, null);
        SetComponent(KeyArrayPosition.GAME_CONTROLLER, null);
        SetComponent(KeyArrayPosition.TIME_CONTROLLER, null);
        SetComponent(KeyArrayPosition.BEAT_CONTROLLER, null);
    }

    public ILevelLoaderController GetLevelLoaderController()
    {
        return this.levelLoaderController;
    }

    public IGameController GetGameController()
    {
        return this.gameController;
    }

    public ITimeController GetTimeController()
    {
        return this.timeController;
    }

    public IBeatController GetBeatController()
    {
        return this.beatController;
    }

    public void SetInjection(ILevelLoaderController levelLoaderInjection)
    {
        this.levelLoaderController = levelLoaderInjection;
        SetComponent(KeyArrayPosition.LEVEL_LOADER_CONTROLLER, levelLoaderInjection);
    }

    public void SetInjection(IGameController gameControllerInjection)
    {
        this.gameController = gameControllerInjection;
        SetComponent(KeyArrayPosition.GAME_CONTROLLER, gameControllerInjection);
    }

    public void SetInjection(ITimeController timeControllerInjection)
    {
        this.timeController = timeControllerInjection;
        SetComponent(KeyArrayPosition.TIME_CONTROLLER, timeControllerInjection);
    }

    public void SetInjection(IBeatController beatControllerInjection)
    {
        this.beatController = beatControllerInjection;
        SetComponent(KeyArrayPosition.BEAT_CONTROLLER, beatControllerInjection);
    }

}
