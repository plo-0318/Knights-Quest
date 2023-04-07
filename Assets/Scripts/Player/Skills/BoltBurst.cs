using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltBurst : MonoBehaviour
{
    private CircleCollider2D col;
    private float damage;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();

        DisableCollider();
    }

    public void Init(float damage)
    {
        this.damage = damage;
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
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Hurt(damage, GameManager.SoundManager().audioRefs.sfxBoltBurst);
        }
    }
}
