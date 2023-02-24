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
        Debug.Log("mouse pos: " + Camera.main.ScreenToWorldPoint(gatherInput.mousePos));
        // Debug.Log("player pos: " + playerTrans.position);

        Rotate();
    }

    private void Rotate() { 
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(gatherInput.mousePos); //+ Vector3.forward * 10f);
         
        //angle between mouse position and object
        float angle = AngleBetweenPoints(transform.position, mouseWorldPosition);
         
        //rotate object towards mouse
        transform.rotation =  Quaternion.Euler (new Vector3(0f,0f,angle));
    }

    float AngleBetweenPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
