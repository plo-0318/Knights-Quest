using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBarrelSpawner : MonoBehaviour
{
    public GameObject barrelPref;
    public Transform player; 
    public float radius = 10f;
    public float interval = 4f;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnBarrel), 0f, interval);
    }
    
    private void SpawnBarrel(){
        Vector2 randomDirection = Random.insideUnitCircle.normalized * radius;
        Vector3 spawnPos = new Vector3(player.position.x + randomDirection.x, player.position.y + randomDirection.y, player.position.z);

        GameObject spawnedBarrel = Instantiate(barrelPref, spawnPos, Quaternion.identity);

         CollectableSpawner collectableSpawner = spawnedBarrel.GetComponent<CollectableSpawner>();
        if (collectableSpawner != null)
        {
            collectableSpawner.SpawnRandomCollectable(spawnPos, Quaternion.identity);
        }
    }

}

