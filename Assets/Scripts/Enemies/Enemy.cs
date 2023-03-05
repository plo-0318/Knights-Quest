using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected GameSession gameSession;
    protected Transform playerTrans;
    protected Rigidbody2D rb;

    protected virtual void Awake() { }

    protected virtual void Start()
    {
        gameSession = GameManager.GameSession();
        playerTrans = GameManager.PlayerMovement().transform;
        rb = GetComponent<Rigidbody2D>();

        gameSession.OnEnemySpawn();
        gameSession.OnKillAllEnemies += ProcessDeath;
    }

    protected virtual void FixedUpdate()
    {
        Flip();
    }

    protected virtual void OnDestroy()
    {
        gameSession.OnEnemyDestroy();
        gameSession.OnKillAllEnemies -= ProcessDeath;
    }

    protected void Flip()
    {
        Vector3 newScale = new Vector3(
            playerTrans.position.x < transform.position.x ? -1f : 1f,
            transform.localScale.y,
            transform.localScale.z
        );

        transform.localScale = newScale;
    }

    protected void ProcessDeath()
    {
        Destroy(gameObject);
    }
}
