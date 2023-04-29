using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnerManager : MonoBehaviour
{
    private GameSession gameSession;

    [SerializeField]
    private GameObject enemySpawner;

    // [SerializeField]
    // private int numSpawners = 8;

    [Tooltip("The distance between the spawners and the player")]
    [SerializeField]
    private float distance = 10f;

    private List<GameObject> spawners;

    private void Awake()
    {
        GameManager.RegisterSpawnerManager(this);

        spawners = new List<GameObject>();
    }

    private void Start()
    {
        // InstantiateSpawners();
    }

    private void RemoveAllSpawners()
    {
        foreach (GameObject spawner in spawners)
        {
            Destroy(spawner.gameObject);
        }

        spawners.Clear();
    }

    public void InstantiateSpawners(int numSpawners)
    {
        RemoveAllSpawners();

        float degBetweenSpawner = 360f / numSpawners;
        float currentDeg = 0f;

        for (int i = 0; i < numSpawners; i++)
        {
            Vector3 spawnPos = new Vector3(
                transform.position.x + Mathf.Cos(currentDeg * Mathf.Deg2Rad) * distance,
                transform.position.y + Mathf.Sin(currentDeg * Mathf.Deg2Rad) * distance,
                transform.position.z
            );

            GameObject spawner = Instantiate(
                enemySpawner,
                spawnPos,
                Quaternion.identity,
                transform
            );

            spawners.Add(spawner);

            currentDeg += degBetweenSpawner;
        }
    }

    // Try to spawn an elite enemy at a random spawner's position
    public void SpawnEliteEnemy()
    {
        // Get the spawners and save them to a temp list
        List<GameObject> tempSpawners = new List<GameObject>(spawners);

        // Shuffle the list
        Util.ShuffleList<GameObject>(tempSpawners);

        // If no spawners, abort
        if (tempSpawners.Count <= 0)
        {
            return;
        }

        // Get the next elite enemy to spawn
        Enemy enemy = EnemySpawnUtil.NextEliteEnemyToSpawn();

        // If cannot get valid enemy, abort
        if (enemy == null)
        {
            return;
        }

        // Iterate through the spawner, if the spawner is inside the map, spawn the elite enemy
        for (int i = 0; i < tempSpawners.Count; i++)
        {
            // Get the current spawner
            GameObject spawner = tempSpawners[i];

            // If spawner is not valid, skil this iteration
            if (spawner == null)
            {
                continue;
            }

            // If the spawner is valid AND inside the map, spawn the elite enemy
            if (spawner.GetComponent<EnemySpawner>().InsideMap())
            {
                Instantiate(
                    enemy,
                    spawner.transform.position,
                    Quaternion.identity,
                    GameManager.GameSession().enemyParent
                );

                return;
            }
        }

        // Not able to spawn an elite enemy, decrement the elite enemy index to its original value
        EnemySpawnUtil.DecrementEliteEnemyIndex();
    }
}
