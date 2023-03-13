using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected GameSession gameSession;
    protected Transform playerTrans;
    protected Rigidbody2D rb;
    protected Stat _stat;

    protected virtual void Awake()
    {
        _stat = new Stat();
    }

    protected virtual void Start()
    {
        gameSession = GameManager.GameSession();
        playerTrans = GameManager.PlayerMovement().transform;
        rb = GetComponent<Rigidbody2D>();

        gameSession.OnEnemySpawn(this);
        gameSession.OnKillAllEnemies += ProcessDeath;
        gameSession.OnRemoveModifier += stat.RemoveModifier;
    }

    protected virtual void FixedUpdate()
    {
        Flip();
    }

    protected virtual void OnDestroy()
    {
        gameSession.OnEnemyDestroy(this);
        gameSession.OnKillAllEnemies -= ProcessDeath;
        gameSession.OnRemoveModifier -= stat.RemoveModifier;
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

    public virtual void Init(Modifier[] modifiers)
    {
        Stat.ApplyModifiers(_stat, modifiers);
    }

    public Stat stat => _stat;
}
