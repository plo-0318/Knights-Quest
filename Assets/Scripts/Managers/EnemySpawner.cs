using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameSession gameSession;
    private float spawnCooldownTime;
    private float spawnCooldownTimer;
    private float spawnTimeOffset;

    private void Start()
    {
        gameSession = GameManager.GameSession();

        gameSession.SpawnEnemy += Spawn;

        spawnCooldownTime = 1f;
        spawnCooldownTimer = 0;
        spawnTimeOffset = Random.Range(1, 6) * 0.1f;
    }

    private void Update()
    {
        spawnCooldownTimer += Time.deltaTime;
    }

    private void OnDestroy()
    {
        gameSession.SpawnEnemy -= Spawn;
    }

    private void Spawn(Enemy enemy)
    {
        if (spawnCooldownTimer < spawnCooldownTime + spawnTimeOffset)
        {
            return;
        }

        spawnCooldownTimer = 0;
        Enemy newEnemy = Instantiate(enemy, transform.position, Quaternion.identity);

        newEnemy.transform.parent = gameSession.enemyParent;
    }
}
