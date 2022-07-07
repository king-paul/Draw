using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VRButton : MonoBehaviour
{
    public UnityEvent onClick;

    public void Activate()
    {
        onClick.Invoke();
    }
}
