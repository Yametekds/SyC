using UnityEngine;


public class MainMenuBeatController : BeatController, IMainMenuHubDependecy
{
    
    private ISongSelector songSelector;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        this.songSelector.OnMenuSongChange -= OnSongChage;
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
        this.songSelector.OnMenuSongChange += OnSongChage;
    }
    
    private void OnSongChage(MainMenuSong song)
    {
        SongHeader songHeader = song.GetSong();
        SetBeatPerMinute(songHeader.GetMainBpm());
    }

}
