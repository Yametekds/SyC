using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISongSelector : IInjection
{
    public event Action<MainMenuSong> OnMenuSongChange;
    public void Play(MainMenuSong song);
}   
