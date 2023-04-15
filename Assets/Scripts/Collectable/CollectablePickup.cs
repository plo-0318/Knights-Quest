using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class CollectablePickup : MonoBehaviour
{
    [SerializeField]
    private Transform absorbTransform;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<Collectable>(out Collectable collectable))
        {
            collectable.PickUp(absorbTransform);
        }
    }
}
