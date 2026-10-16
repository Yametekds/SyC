
using System.Collections;
using System.Collections.Generic;
using System;


public interface IGameController : IInjection
{
    public event Action<int> OnSongCountDown;
    public event Action OnSongStart;
    public event Action<NoteHolder> OnGoodNote;
    public event Action<int> OnTotalPointsUp;
    public event Action<int> OnTotalPointsDown;
    public event Action<int> OnSongFinish;

    public void FinishSong();
}
