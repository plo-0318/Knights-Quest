using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameSession gameSession;
    private SpawnerManager spawnerManager;
    private float spawnCooldownTime;
    private float spawnCooldownTimer;
    private float spawnTimeOffset;
    private MapConfiner mapConfiner;

    private void Start()
    {
        gameSession = GameManager.GameSession();
        mapConfiner = GameManager.MapConfiner();

        gameSession.onSpawnEnemy += Spawn;

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
        gameSession.onSpawnEnemy -= Spawn;
    }

    private void Spawn(EnemySpawnUtil.EnemyToSpawn enemyToSpawn, Modifier[] enemyModifiers)
    {
        // If spawn is on cooldown, return
        if (spawnCooldownTimer < spawnCooldownTime + spawnTimeOffset)
        {
            return;
        }

        // If spawn point is outside of the map, return
        if (!mapConfiner.InsideMap(transform.position))
        {
            return;
        }

        // Reset spawn timer, and instantiate the enemy game object
        spawnCooldownTimer = 0;

        Enemy newEnemy = Instantiate(enemyToSpawn(), transform.position, Quaternion.identity);

        // If there is a global enemy modifier, apply it
        if (enemyModifiers != null && enemyModifiers.Length > 0)
        {
            newEnemy.Init(enemyModifiers);
        }

        // Setting the enemy parent
        newEnemy.transform.parent = gameSession.enemyParent;
    }
}
