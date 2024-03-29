using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class speedField : MonoBehaviour
{
    private Modifier speedMod;
    public float speedMultiplier = 0.5f;

    private void Start()
    {
        speedMod = new Modifier(Stat.SPEED, "SpeedFieldTest", speedMultiplier);

        // Destroy(gameObject, UnityEngine.Random.Range(8f, 20f));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.AddModifier(speedMod, false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.RemoveModifier(speedMod);
        }
    }

    private void OnDestroy()
    {
        GameManager.GameSession().RemoveModifierFromAllEnemies(speedMod);
    }
}
