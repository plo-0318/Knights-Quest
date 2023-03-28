using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableAbsorb : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<Collectable>(out Collectable collectable))
        {
            collectable.Use();
        }
    }
}
