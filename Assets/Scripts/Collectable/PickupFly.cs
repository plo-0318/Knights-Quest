using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFly : MonoBehaviour
{
    public float PickupSpeed = 2f;
    private GameObject player;
    public bool inRadius = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (inRadius == true)
        {
            MoveTowardsPlayer();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRadius = true;
        }
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector3.Lerp(
            this.transform.position,
            player.transform.position,
            PickupSpeed * Time.deltaTime
        );
    }
}
