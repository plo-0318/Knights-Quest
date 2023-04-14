using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollectableBarrelSpawner : MonoBehaviour
{
    public GameObject barrelPref;
    public Transform player; 
    public float minRadius = 10f;
    public float maxRadius = 15f;
    public float interval = 4f;
    public Tilemap gameMap;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnBarrel), 0f, interval);
    }
    
    private void SpawnBarrel(){
        float radius = Random.Range(minRadius, maxRadius);
        Vector2 randomDirection = Random.insideUnitCircle.normalized * radius;
        Vector3 spawnPos = new Vector3(player.position.x + randomDirection.x, player.position.y + randomDirection.y, player.position.z);

        if(IsWithinBounds(spawnPos))
        {
            GameObject spawnedBarrel = Instantiate(barrelPref, spawnPos, Quaternion.identity);

            CollectableSpawner collectableSpawner = spawnedBarrel.GetComponent<CollectableSpawner>();
            if (collectableSpawner != null)
            {
                collectableSpawner.SpawnRandomCollectable(spawnPos, Quaternion.identity);
            }
        }
    }

    private bool IsWithinBounds(Vector3 position)
    {
       BoundsInt mapBounds = gameMap.cellBounds;
       Vector3Int cellPosition = gameMap.WorldToCell(position);

       return mapBounds.Contains(cellPosition);
    }

}

