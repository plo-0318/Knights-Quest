using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorb : MonoBehaviour
{
    void Update() { }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Absorb"))
        {
            Destroy(gameObject);
        }
    }
}
