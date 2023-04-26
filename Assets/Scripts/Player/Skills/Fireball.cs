using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private float damage,
        speed;
    private Vector2 targetPos;
    private Enemy targetEnemy;

    private float destroyTimer;
    private GameObject explosionPrefab;
    private GameObject fieldPrefab;

    private void Awake()
    {
        targetEnemy = null;
        explosionPrefab = null;
        fieldPrefab = null;
    }

    private void Start()
    {
        destroyTimer = 1.5f;
    }

    private void Update()
    {
        if (ReachedTarget())
        {
            SpawnExplosion();
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                CalculateTargetPos(),
                Time.deltaTime * speed
            );

            Rotate();
        }

        if (destroyTimer <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            destroyTimer -= Time.deltaTime;
        }
    }

    public void Init(
        float damage,
        float speed,
        Enemy target,
        Vector2 targetPos,
        GameObject explosionPrefab,
        GameObject fieldPrefab = null
    )
    {
        this.damage = damage;
        this.speed = speed;
        this.targetEnemy = target;
        this.targetPos = targetPos;
        this.explosionPrefab = explosionPrefab;
        this.fieldPrefab = fieldPrefab;
    }

    private Vector3 CalculateTargetPos()
    {
        if (targetEnemy != null)
        {
            targetPos = targetEnemy.transform.position;
        }

        return targetPos;
    }

    private void Rotate()
    {
        float zRotation = Util.GetNormalizedAngle(transform.position, CalculateTargetPos());

        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    private bool ReachedTarget()
    {
        return Vector2.Distance(transform.position, CalculateTargetPos()) < 0.25f;
    }

    private void SpawnExplosion()
    {
        FireballExplosion spawnedExplosion = Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity,
                GameManager.GameSession().skillParent
            )
            .GetComponent<FireballExplosion>();

        spawnedExplosion.Init(damage, fieldPrefab);
    }
}
