using System;

public interface IBeatController : IInjection
{
    public event Action<BeatInfo> OnBeatChange;
    public event Action OnBeatSignal;
    public BeatInfo CurrentBeatInfo { get; }
}
