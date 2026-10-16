using System;
using System.Collections;
using UnityEngine;

public class DetectNoteController : MonoBehaviour
{
    public event Action OnFailNote;
    public event Action<NoteHolder> OnGoodNote;

    public LayerMask notesLayerMask;
    public float rayLongitude = 1f;
    public Transform rayFrontStartPoint;
    public Transform rayMidStartPoint;
    public Transform rayBackStartPoint;


    public float rayOutLongitude = 0.05f;
    public Transform rayOutCheckerStartPoint;

    
    public  Animator haloAnimator;


    public string InputName { get; set; }

    private RaycastHit2D rayHit; 
    private RaycastHit2D rayHitFront; 
    private RaycastHit2D rayHitMid; 
    private RaycastHit2D rayHitBack;


    private RaycastHit2D rayOutHit;
    private bool buttonPress;

    

    private void Update()
    {

        buttonPress = Input.GetButtonDown(this.InputName);
        rayOutHit = Physics2D.Raycast(rayOutCheckerStartPoint.position, Vector2.right, rayOutLongitude, notesLayerMask);
        rayHit = GenerateRayCast();
#if UNITY_EDITOR
        Debug.DrawRay(rayOutCheckerStartPoint.position, rayOutLongitude * Vector2.right, Color.green);
#endif


        if (!buttonPress)
        {
            NoPressButtonChecker();
            return;
        }
        else
        {

            if (rayHit.collider == null)
            {
                ReducePoints();
                return;
            }
            else
            {
                
                NoteHolder note = rayHit.collider.gameObject.GetComponent<NoteHolder>();

                if(note.IsProcess)
                {
                    return;
                }


                note.RegistProcessTime();
                int notePoints = note.RangeZonePoints();
                bool inside = notePoints != 0;
                if (!inside)
                {
                    ReducePoints();
                    return;
                }
                else
                {
                    GoodNotePress(note);
                }
                
            }
        }
    }


    private void NoPressButtonChecker()
    {
       if(rayOutHit.collider != null) 
       {
            NoteHolder note = rayOutHit.collider.gameObject.GetComponent<NoteHolder>();
            int notePoints = note.RangeZonePoints();
            bool outside = notePoints == 0;
            if (!note.IsProcess && outside)
            {
                note.SetAsProcess();
                ReturnToPool(note);
                ReducePoints();
            }
        }
    }

    private void ReducePoints()
    {
        OnFailNote?.Invoke();
    }

    private void GoodNotePress(NoteHolder note)
    {
        note.SetAsProcess();
        ReturnToPool(note);
        note.PlayTone();
        haloAnimator.SetTrigger("add-point");
        OnGoodNote?.Invoke(note);
    }

    private void ReturnToPool(NoteHolder note)
    {
        ObjectFromPool fromPool = note.gameObject.GetComponent<ObjectFromPool>();
        float length = 2f;
        if (fromPool == null)
        {
            Debug.LogError("destroy");
            Destroy(note.gameObject, length);
        }
        else 
        {
            IEnumerator corrutine = ReturnToPool(fromPool, length);
            StartCoroutine(corrutine);
        }

    }

    private IEnumerator ReturnToPool(ObjectFromPool fromPool, float waitForSeconds)
    {
        yield return new WaitForSeconds(waitForSeconds);
        fromPool.ReturnToPool();
    }

    private RaycastHit2D GenerateRayCast() 
    {

#if UNITY_EDITOR
        Debug.DrawRay(rayFrontStartPoint.position, rayLongitude * Vector2.down, Color.green);
        Debug.DrawRay(rayMidStartPoint.position, rayLongitude * Vector2.down, Color.green);
        Debug.DrawRay(rayBackStartPoint.position, rayLongitude * Vector2.down, Color.green);
#endif
        rayHitFront = Physics2D.Raycast(rayFrontStartPoint.position, Vector2.down, rayLongitude, notesLayerMask);
        rayHitMid = Physics2D.Raycast(rayMidStartPoint.position, Vector2.down, rayLongitude, notesLayerMask); 
        rayHitBack = Physics2D.Raycast(rayBackStartPoint.position, Vector2.down, rayLongitude, notesLayerMask);

        if (rayHitFront.collider != null && rayHitFront.collider.gameObject.Equals(rayHitBack.collider != null ? rayHitBack.collider.gameObject : null))
        {
            return rayHitMid;
        }

        if (rayHitMid.collider != null && rayHitMid.collider.gameObject.Equals(rayHitFront.collider != null ? rayHitFront.collider.gameObject : null))
        {
            return rayHitFront;
        }

        if (rayHitMid.collider != null && rayHitMid.collider.gameObject.Equals(rayHitBack.collider != null ? rayHitBack.collider.gameObject : null))
        {
            return rayHitBack;
        }

        if (rayHitMid.collider != null )
        {
            return rayHitMid;
        }

        if (rayHitFront.collider != null)
        {
            return rayHitFront;
        }

        if (rayHitBack.collider != null)
        {
            return rayHitBack;
        }

        return rayHitMid;
    }
}
