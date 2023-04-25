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

    public void SpawnEliteEnemy()
    {
        List<GameObject> tempSpawners = new List<GameObject>(spawners);

        Util.ShuffleList<GameObject>(tempSpawners);

        if (tempSpawners.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < tempSpawners.Count; i++)
        {
            GameObject spawner = tempSpawners[i];

            if (spawner == null)
            {
                return;
            }

            if (spawner.GetComponent<EnemySpawner>().InsideMap())
            {
                Instantiate(
                    EnemySpawnUtil.NextEliteEnemyToSpawn(),
                    spawner.transform.position,
                    Quaternion.identity,
                    GameManager.GameSession().enemyParent
                );

                return;
            }
        }
    }
}
