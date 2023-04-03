using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerMovement : MonoBehaviour
{
    public GameObject player;

    private void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
    }

    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject;
    }
}
