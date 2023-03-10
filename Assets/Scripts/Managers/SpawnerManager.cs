using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField]
    private int numSpawners = 8;

    [Tooltip("The distance between the spawners and the player")]
    [SerializeField]
    private float distance = 10f;

    [SerializeField]
    private GameObject enemySpawner;

    private void Start()
    {
        InstantiateSpawners();
    }

    private void InstantiateSpawners()
    {
        float degBetweenSpawner = 360f / numSpawners;
        float currentDeg = 0f;

        for (int i = 0; i < numSpawners; i++)
        {
            Vector3 spawnPos = new Vector3(
                transform.position.x + Mathf.Cos(currentDeg * Mathf.Deg2Rad) * distance,
                transform.position.y + Mathf.Sin(currentDeg * Mathf.Deg2Rad) * distance,
                transform.position.z
            );

            // Debug.Log("trans: " + transform.position);
            // Debug.Log(
            //     "deg: "
            //         + currentDeg
            //         + ", x: "
            //         + Mathf.Cos(currentDeg) * distance
            //         + ", y: "
            //         + Mathf.Sin(currentDeg) * distance
            //         + ", pos: "
            //         + spawnPos
            // );

            GameObject spawner = Instantiate(enemySpawner, spawnPos, Quaternion.identity);

            spawner.transform.parent = transform;

            currentDeg += degBetweenSpawner;
        }
    }
}
