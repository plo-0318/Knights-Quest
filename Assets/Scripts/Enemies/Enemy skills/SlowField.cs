using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowField : MonoBehaviour
{
    private Modifier speedMod;

    private PlayerStatus playerStatus;

    [SerializeField]
    private float aliveTime;

    private void Awake()
    {
        speedMod = new Modifier(Stat.SPEED, "BossSlowField", -0.75f);
    }

    private void Start()
    {
        playerStatus = GameManager.PlayerStatus();
    }

    private void Update()
    {
        if (aliveTime <= 0)
        {
            Destroy(gameObject);
            return;
        }

        aliveTime -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        playerStatus.RemoveModifier(speedMod);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerCollider>() != null)
        {
            playerStatus.AddModifier(speedMod);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerCollider>() != null)
        {
            playerStatus.RemoveModifier(speedMod);
        }
    }
}
