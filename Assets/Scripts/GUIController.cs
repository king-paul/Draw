using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    public Transform camera;

    private RaycastHit hit;
    private float maxCastDiscatance = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RayCastToGUI()
    {
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxCastDiscatance, 5)) // 5 = UI Layer
        {
            if (hit.transform.gameObject.layer == 5)
            {
                Debug.Log("Clicked on " + hit.transform.name);
                //Button selected = hit.transform.GetComponent<Button>();
                //selected.onClick.Invoke();
            }
        }
    }

    public void DebugMessage()
    {
        Debug.Log("Start button clicked");
    }
}
