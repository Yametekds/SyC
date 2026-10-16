using UnityEngine;

public class MainMenuSong {

    private AudioClip audioClip;
    private SongHeader song;
    private Sprite background;

    public MainMenuSong()
    {

    }

    public MainMenuSong(SongHeader header, AudioClip audioClip)
    {
        this.song = header;
        this.audioClip = audioClip;
    }

    public AudioClip GetAudioClip()
    {
        return audioClip;
    }

    public void SetAudioClip(AudioClip audioClip)
    {
        this.audioClip = audioClip;
    }

    public void SetSong(SongHeader song) 
    {
        this.song = song;
    }

    public SongHeader GetSong() 
    {
        return song;
    }

    public void SetBackground(Sprite background)
    {
        this.background = background;
    }

    public Sprite GetBackground()
    {
        return background;
    }
}
