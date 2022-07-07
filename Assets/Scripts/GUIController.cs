using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    //public Transform camera;
    //public Transform leftController;
    public Transform rightController;
    //public Transform leftFiringPoint;
    public Transform rightFiringPoint;
    //public Transform leftCursor;
    public Transform rightCursor;
    [SerializeField] float maxCastDiscatance = 50f;
    public LineRenderer line;

    public AudioClip titleMusicLoop;
    [SerializeField] bool playTitleMusic;

    AudioSource audio;

    private RaycastHit hit;    

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        line.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (audio != null && !audio.isPlaying && titleMusicLoop != null)
        {
            audio.clip = titleMusicLoop;
            audio.loop = true;
            audio.Play();
        }

        // draw line renderer
        line.SetPosition(0, rightFiringPoint.position);

        Vector3 fireDirection = (rightCursor.position - rightFiringPoint.position).normalized;
        if (Physics.Raycast(rightController.position, fireDirection, out hit))
        {
            line.SetPosition(1, hit.point);
            //Debug.Log("Raycast is hitting: " + hit.transform.name);
        }
        else
        {
            line.SetPosition(1, rightCursor.position);
        }

        //Debug.DrawRay(rightFiringPoint.position, fireDirection);
    }

    public void RayCastToGUI()
    {
        Vector3 fireDirection = (rightCursor.position - rightFiringPoint.position).normalized;        

        if (Physics.Raycast(rightFiringPoint.position, fireDirection, out hit, maxCastDiscatance)) 
        {       
            if (hit.transform.gameObject.layer == 5)  // 5 = UI Layer
            {
                Debug.Log("Clicked on " + hit.transform.name);
                hit.transform.GetComponent<VRButton>().Activate();
            }
            
        }
    }

    public void DebugMessage()
    {
        Debug.Log("right trigger pressed");
    }
}
