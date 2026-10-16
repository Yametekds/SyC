using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatControllerGameplay : BeatController, IGameHubDependecy
{

    private ITimeController timeController;
    private ILevelLoaderController levelLoader;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        this.levelLoader.OnLoadComplete -= OnLevelLoadedComplete;
    }

    public override void BeatUpdate()
    {
        this.NextBeat += timeController.GetDeltaTime();
    }

    public void SetInjection(IGameHubComponentController injection)
    {
        injection.OnLoadComplete += this.OnHubCompleteLoad;
    }

    private void OnHubCompleteLoad(IGameHubComponentController hub)
    {
        hub.OnLoadComplete -= this.OnHubCompleteLoad;
        SetLevelLoadController(hub.GetLevelLoaderController());
        SetTimeController(hub.GetTimeController());
    }

    private void SetTimeController(ITimeController injection)
    {
        this.timeController = injection;
    }

    private void SetLevelLoadController(ILevelLoaderController injection)
    {
        this.levelLoader = injection;
        this.levelLoader.OnLoadComplete += OnLevelLoadedComplete;
    }

    private void OnLevelLoadedComplete(LevelModel level)
    {
        SongWithDetails song = level.GetSongWithDetails();
        base.SetBeatPerMinute(song.GetMainBpm());
    }

}
