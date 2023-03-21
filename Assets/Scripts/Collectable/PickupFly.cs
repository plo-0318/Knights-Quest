using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFly : MonoBehaviour
{
    public float PickupSpeed;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                other.transform.position,
                PickupSpeed * Time.deltaTime
            );
        }
    }
}
