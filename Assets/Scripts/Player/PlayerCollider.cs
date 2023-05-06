using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            float damage = enemy.GetStat(Stat.DAMAGE);
            Vector2 direction = transform.position - enemy.transform.position;

            GameManager.PlayerStatus().Hurt(damage, direction, enemy);
        }
    }
}
