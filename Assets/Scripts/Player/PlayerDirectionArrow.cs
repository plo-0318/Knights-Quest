using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionArrow : MonoBehaviour
{
    private Transform playerTrans;
    private GatherInput gatherInput;

    [SerializeField]
    private float rotateSpeed = 5f;
    private float radius = 1f;
    private const float DEFAULT_ROTATE_SPEED_MULTIPLYER = 50f;

    private void Start()
    {
        playerTrans = transform.parent.transform;
        gatherInput = playerTrans.GetComponent<GatherInput>();
    }

    private void FixedUpdate()
    {
        // Debug.Log("mouse pos: " + Camera.main.ScreenToWorldPoint(gatherInput.mousePos));
        // Debug.Log("player pos: " + playerTrans.position);

        Rotate();
    }

    private void Rotate() { }
}
