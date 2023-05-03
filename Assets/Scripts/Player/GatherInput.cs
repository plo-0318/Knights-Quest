using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GatherInput : MonoBehaviour
{
    private Control myControl;

    [System.NonSerialized]
    public float moveValueX,
        moveValueY;

    public Vector2 mousePos;
    public event Action onPausePressed;

    private void Awake()
    {
        myControl = new Control();

        GameManager.RegisterGatherInput(this);
    }

    private void OnEnable()
    {
        myControl.Player.MoveX.performed += StartMoveX;
        myControl.Player.MoveX.canceled += StopMoveX;

        myControl.Player.MoveY.performed += StartMoveY;
        myControl.Player.MoveY.canceled += StopMoveY;

        myControl.Player.Mouse.performed += MouseMove;

        myControl.UI.Pause.performed += HandlePause;

        myControl.Player.Enable();
        myControl.UI.Enable();
    }

    private void OnDisable()
    {
        DisableControls();
    }

    private void HandlePause(InputAction.CallbackContext ctx)
    {
        onPausePressed?.Invoke();
    }

    private void StartMoveX(InputAction.CallbackContext ctx)
    {
        moveValueX = ctx.ReadValue<float>();
    }

    private void StopMoveX(InputAction.CallbackContext ctx)
    {
        moveValueX = 0;
    }

    private void StartMoveY(InputAction.CallbackContext ctx)
    {
        moveValueY = ctx.ReadValue<float>();
    }

    private void StopMoveY(InputAction.CallbackContext ctx)
    {
        moveValueY = 0;
    }

    private void MouseMove(InputAction.CallbackContext ctx)
    {
        mousePos = ctx.ReadValue<Vector2>();
    }

    public void DisableControls()
    {
        myControl.Player.MoveX.performed -= StartMoveX;
        myControl.Player.MoveX.canceled -= StopMoveX;

        myControl.Player.MoveY.performed -= StartMoveY;
        myControl.Player.MoveY.canceled -= StopMoveY;

        myControl.Player.Mouse.performed -= MouseMove;

        myControl.UI.Pause.performed += HandlePause;

        myControl.Player.Disable();
        myControl.UI.Disable();

        moveValueX = moveValueY = 0;
    }
}
