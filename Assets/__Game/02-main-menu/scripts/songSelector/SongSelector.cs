using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSelector : MonoBehaviour, ISongSelector, IMainMenuHubDependecy
{
    public event Action<MainMenuSong> OnMenuSongChange;
    private IMainMenuKeyboardController buttonController;

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

    public void SetKeyBoardController(IMainMenuKeyboardController injection)
    {
        this.buttonController = injection;
        this.buttonController.OnButtonPress += ButtonsListener;
    }

    public void Play(MainMenuSong song)
    {
        if (song != null && song.GetAudioClip() != null)
        {
            OnMenuSongChange?.Invoke(song);
        }
    }

    public void NextSong()
    {
        MainMenuSong song = ResourcesContainer.GetNext();
        Play(song);
    }

    public void PreviusSong()
    {
        MainMenuSong song = ResourcesContainer.GetPrevius();
        Play(song);
    }


    private void ButtonsListener(IMainMenuKeyboardController.GameKeys key)
    {
        switch (key)
        {
            case IMainMenuKeyboardController.GameKeys.leftButton: PreviusSong(); break;
            case IMainMenuKeyboardController.GameKeys.rigthButton: NextSong(); break;
        }
    }

    
}
