using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Mouse Options")]
    public bool lockMouseCursor = true;

    [Header("Events")]
    [Space]
    public UnityEvent onLeftTriggerPressed;
    public UnityEvent onLeftTriggerReleased;
    public UnityEvent onRightTriggerPressed;
    public UnityEvent onRightTriggerReleased;

    public UnityEvent onAButton;
    public UnityEvent onBButton;
    public UnityEvent onXButton;
    public UnityEvent onYButton;

    [Header("Right Trigger")]    
    [Space]
    [Header("Input Mapping")]
    public InputAction rightTrigger;
    [Header("A Button")]
    public InputAction rightPrimary;
    [Header("B Button")]
    public InputAction rightSecondary;

    [Space]
    [Header("Left Trigger")]
    public InputAction leftTrigger;
    [Header("X Button")]
    public InputAction leftPrimary;
    [Header("Y Button")]
    public InputAction leftSecondary;
    

    // Start is called before the first frame update
    void Start()
    {
        if(lockMouseCursor)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        // link input actions to unity events
        rightTrigger.performed += action => onRightTriggerPressed.Invoke();
        rightTrigger.canceled += action => onRightTriggerReleased.Invoke();
        rightTrigger.Enable();

        rightPrimary.performed += action => onAButton.Invoke();
        rightPrimary.Enable();

        rightSecondary.performed += action => onBButton.Invoke();
        rightSecondary.Enable();

        leftTrigger.performed += action => onLeftTriggerPressed.Invoke();
        leftTrigger.canceled += action => onLeftTriggerReleased.Invoke();
        leftTrigger.Enable();

        leftPrimary.performed += action => onXButton.Invoke();
        leftPrimary.Enable();

        leftSecondary.performed += action => onYButton.Invoke();
        leftSecondary.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}