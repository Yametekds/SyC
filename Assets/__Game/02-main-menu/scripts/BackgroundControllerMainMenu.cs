using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundControllerMainMenu : MonoBehaviour, IMainMenuHubDependecy
{
    public UnityEngine.UI.Image image;
    //public SpriteRenderer spriteRenderer;
    private ISongSelector songSelector;

    private void OnDestroy()
    {
        this.songSelector.OnMenuSongChange -= OnSongChangeListener;
    }

    public void SetInjection(IMainMenuHubComponentController injection)
    {
        injection.OnLoadComplete += this.OnHubCompleteLoad;
    }

    private void OnHubCompleteLoad(IMainMenuHubComponentController hub)
    {
        hub.OnLoadComplete -= this.OnHubCompleteLoad;
        SetSongSelectorController(hub.GetSongSelectorController());
    }

    public void SetSongSelectorController(ISongSelector injection)
    {
        this.songSelector = injection;
        this.songSelector.OnMenuSongChange += OnSongChangeListener;
    }

    private void OnSongChangeListener(MainMenuSong song) 
    {
        /*
        if(spriteRenderer != null) 
        {
            spriteRenderer.sprite = song.GetBackground();
        }
        */
        if (image != null)
        {
            image.sprite = song.GetBackground();
        }
    }
}
