using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedHazard : MonoBehaviour
{
    private Collider2D col;

    [SerializeField]
    private float damage;
    private bool canDamage;

    private void Awake()
    {
        col = GetComponent<Collider2D>();

        DisableCollider();

        canDamage = true;
    }

    private void EnableCollider()
    {
        col.enabled = true;
    }

    private void DisableCollider()
    {
        col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canDamage)
        {
            return;
        }

        if (other.GetComponent<PlayerCollider>() != null)
        {
            GameManager.PlayerStatus().Hurt(damage);

            canDamage = false;
        }
    }
}
