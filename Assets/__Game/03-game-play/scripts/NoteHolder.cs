using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHolder : MonoBehaviour
{
    public float proccessTime = -1.0f;

    
    public Vector2 initialPosition;

    private static ITimeController timeController;
    private static bool beatChange = false;
    private static BeatInfo beatInfo;
    private static float speed;
    private static float translationTime;


    private Transform mTransform;
    private SpriteRenderer spriteRenderer;
    private Collider2D mCollider;
    private AudioSource audioSource;

    private DataNote dataNote;
    public int Id { get { return dataNote.id; } }
    public int KeyboardButton { get { return dataNote.key; } set { dataNote.key = value; } }
    public bool IsProcess { get { return dataNote.isProcess; } set { dataNote.isProcess = value; } }

    public float PlayTime { get { return dataNote.spawnTime + NoteHolder.translationTime; } }
    public float OffsetMomentTime { get { return dataNote.hitOffset; } }

    private float minValue;
    private float maxValue;

    private Vector2 mPosition;
    private bool beatStatus = false;

    private void Awake()
    {
        mTransform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (NoteHolder.beatInfo == null || this.dataNote == null)
        {
            return;
        }

        if (this.beatStatus != NoteHolder.beatChange)
        {
            SetupRange();
            this.beatStatus = NoteHolder.beatChange;
        }

        mPosition.x = mTransform.position.x - (Time.deltaTime * NoteHolder.speed);
        mPosition.y = initialPosition.y;
        mTransform.position = mPosition;
    }


    public static void SetTimeController(ITimeController timeController)
    {
        NoteHolder.timeController = timeController;
    }

    public static void SetBeatInfo(BeatInfo beatInfo)
    {
        NoteHolder.beatInfo = beatInfo;
        NoteHolder.beatChange = !NoteHolder.beatChange;
    }

    public static void SetSpeed(float speed)
    {
        NoteHolder.speed = speed;
    }

    public static void SetTranslationTIme(float translationTime)
    {
        NoteHolder.translationTime = translationTime;
    }

    public void SetAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    public void PlayTone()
    {
        audioSource.Play();
    }

    public void BindHolder(DataNote data)
    {
        this.dataNote = data;
        this.beatStatus = NoteHolder.beatChange;
        SetupRange();
    }

    public void RegistProcessTime()
    {
        this.proccessTime = timeController.GetTime();
    }

    public int RangeZonePoints()
    {
        float currentTime = this.proccessTime;
        bool insideRange1 = (currentTime >= this.minValue && currentTime <= this.maxValue);
        return insideRange1 ? 1 : 0;
    }

    public void SetAsProcess()
    {
        this.IsProcess = true;
        mCollider.enabled = false;
        spriteRenderer.enabled = false;
    }

    public void SetupNote()
    {
        this.IsProcess = false;
        mCollider.enabled = true;
        spriteRenderer.enabled = true;
    }

    private void SetupRange()
    {
        minValue = PlayTime - OffsetMomentTime;
        maxValue = PlayTime + OffsetMomentTime;
    }


}