using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IGameHubComponentController : IInjection
{
    public event Action<IGameHubComponentController> OnLoadComplete;

    ILevelLoaderController GetLevelLoaderController();
    IGameController GetGameController();
    ITimeController GetTimeController();
    IBeatController GetBeatController();
}
