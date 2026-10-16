using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMusicLoader : MonoBehaviour, IGameHubDependecy
{
    private ILevelLoaderController levelLoader;
    private IGameController gameController;
    private AudioClip clip;
    private AudioSource baseSong;

    public void SetAudioClip(AudioClip clip)
    {
        this.clip = clip;
    }

    private void Awake()
    {
        baseSong = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        this.gameController.OnSongStart -= PlaySong;
        this.levelLoader.OnLoadComplete -= SetLevelModel;
    }

    public void SetInjection(IGameHubComponentController injection)
    {

        injection.OnLoadComplete += this.OnHubCompleteLoad;
    }

    private void OnHubCompleteLoad(IGameHubComponentController hub)
    {
        hub.OnLoadComplete -= this.OnHubCompleteLoad;
        SetLevelLoadController(hub.GetLevelLoaderController());
        SetGameController(hub.GetGameController());
    }

    private void SetGameController(IGameController gameController)
    {
        this.gameController = gameController;
        this.gameController.OnSongStart += PlaySong;
    }

    private void SetLevelLoadController(ILevelLoaderController injection)
    {
        this.levelLoader = injection;
        this.levelLoader.OnLoadComplete += SetLevelModel;
    }

    private void SetLevelModel(LevelModel level)
    {
        SetAudioClip(level.GetBaseClip());
    }

    private void PlaySong()
    {
        baseSong.clip = clip;
        baseSong.Play();
    }
}
