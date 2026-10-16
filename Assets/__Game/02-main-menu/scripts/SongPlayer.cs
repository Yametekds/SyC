using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongPlayer : MonoBehaviour, IMainMenuHubDependecy
{
    

    [SerializeField] private float waitBetweenSongs = 1.5f;
    [SerializeField] private Text songText;
    [SerializeField] private Text authorText;
    private AudioSource audioSource;
    private ISongSelector songSelector;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        this.songSelector.OnMenuSongChange += OnSongChangeListener;
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

    private void SetSongSelectorController(ISongSelector injection)
    {
        this.songSelector = injection;
        this.songSelector.OnMenuSongChange += OnSongChangeListener;
    }
    
    private void mPlay()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    private void OnSongChangeListener(MainMenuSong song)
    {
        StopAllCoroutines();
        audioSource.Stop();
        SetSong(song);
        StartCoroutine(PlayCoroutine());
    }

    private void SetSong(MainMenuSong song)
    {
        audioSource.clip = song.GetAudioClip();
        var header = song.GetSong();
        songText.text = header.GetName();
        authorText.text = header.GetAuthor();
    }

    private IEnumerator PlayCoroutine()
    {
        yield return new WaitForSeconds(waitBetweenSongs);
        
        mPlay();
    }

    
}
