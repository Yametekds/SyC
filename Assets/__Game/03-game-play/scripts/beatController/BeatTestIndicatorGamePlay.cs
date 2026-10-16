using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTestIndicatorGamePlay : MonoBehaviour, IBeatControllerDependency
{
    private SpriteRenderer image;
    private Sprite sprite;
    private IBeatController beatController;


    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
        sprite = image.sprite;
    }

    private void OnDestroy()
    {
        this.beatController.OnBeatSignal -= OnBeat;
        this.beatController.OnBeatChange -= OnBPMChange;
    }

    public void OnBeat()
    {
        image.sprite = image.sprite == null ? sprite : null;
    }

    public void OnBPMChange(BeatInfo beatInfo)
    {

    }

    public void SetInjection(IBeatController injection)
    {
        this.beatController = injection;
        this.beatController.OnBeatSignal += OnBeat;
        this.beatController.OnBeatChange += OnBPMChange;

    }
}