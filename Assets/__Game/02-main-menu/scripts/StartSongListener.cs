using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartSongListener : MonoBehaviour, IMainMenuHubDependecy
{
    public IMainMenuKeyboardController buttonController;
    public UnityEvent OnPlaySongActionStart;


    private void OnDestroy()
    {
        this.buttonController.OnButtonPress -= ButtonsListener;
    }

    public void SetInjection(IMainMenuHubComponentController injection)
    {
        injection.OnLoadComplete += this.OnHubCompleteLoad;
    }

    private void OnHubCompleteLoad(IMainMenuHubComponentController hub)
    {
        hub.OnLoadComplete -= this.OnHubCompleteLoad;
        SetKeyBoardController(hub.GetKeyBoardController());
    }

    private void SetKeyBoardController(IMainMenuKeyboardController injection)
    {
        this.buttonController = injection;
        this.buttonController.OnButtonPress += ButtonsListener;
    }

    private void ButtonsListener(IMainMenuKeyboardController.GameKeys arg0)
    {
        if( arg0 == IMainMenuKeyboardController.GameKeys.startSongButton)
        {
            OnPlaySongActionStart?.Invoke();
        } 
    }

}
