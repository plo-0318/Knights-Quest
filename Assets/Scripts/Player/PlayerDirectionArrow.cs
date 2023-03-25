using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionArrow : MonoBehaviour
{
    private Transform playerTrans;
    private GatherInput gatherInput;

    // private float radius = 1f;

    private void Awake()
    {
        GameManager.RegisterPlayerDirectionArrow(this);
    }

    private void Start()
    {
        playerTrans = transform.parent.transform;
        gatherInput = playerTrans.GetComponent<GatherInput>();
    }

    private void FixedUpdate()
    {
        // Debug.Log("mouse pos: " + Camera.main.ScreenToWorldPoint(gatherInput.mousePos));
        // Debug.Log("player pos: " + playerTrans.position);

        //Rotate();
        AngleBetweenMouseAndPlayer();
    }

    // public static void Rotate()
    // {
    //     Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(gatherInput.mousePos); //+ Vector3.forward * 10f);

    //     //angle between mouse position and object
    //     float angle = AngleBetweenPoints(transform.position, mouseWorldPosition);

    //     //rotate object towards mouse
    //     transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + 90));
    // }

    // float AngleBetweenPoints(Vector3 a, Vector3 b)
    // {
    //     return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    // }

    public static float AngleBetweenMouseAndPlayer()
    {
        Vector3 mousePos = GameManager.PlayerMovement().MousePos();
        Vector3 playerPos = GameManager.PlayerMovement().transform.position;

        return Mathf.Atan2(playerPos.y - mousePos.y, playerPos.x - mousePos.x) * Mathf.Rad2Deg; 
    }
}
