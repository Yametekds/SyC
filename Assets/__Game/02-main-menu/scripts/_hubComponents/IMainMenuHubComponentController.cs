using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IMainMenuHubComponentController:IInjection
{
    public event Action<IMainMenuHubComponentController> OnLoadComplete;

    public IMainMenuKeyboardController GetKeyBoardController();
    public ISongSelector GetSongSelectorController();
    public IBeatController GetBeatController();

}
