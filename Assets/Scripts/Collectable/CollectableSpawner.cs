using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectableSpawner : MonoBehaviour
{
    [System.Serializable]
    private class RandomCollectables
    {
        [SerializeField]
        [Tooltip("The type of collectable to be spawned")]
        public Collectable.Type collectableType;

        [SerializeField]
        [Tooltip(
            "The chance of this type of collectable spawning. This field only matters if there are more than 1 collectables. The chance of spawning will be calculated based on the total chance."
        )]
        [Range(1, 100)]
        public int spawnChance = 1;
    }

    [SerializeField]
    private RandomCollectables[] collectables;

    // Logic example --> green gem (3), blue gem (4), orange gem (6)
    // Store the chances with each adding the previous chance --> [3 + 0 = 3, 4 + 3 = 7, 6 + 7 = 13] --> [3, 7, 13]
    // Calculate total chance --> 3 + 4 + 6 = 13
    // Generate random value between 1 and total chance (inclusive)
    // 1 to 3 --> green gem, 4 to 7 --> blue gem, 8 to 13 --> orange gem
    private Collectable GetRandomCollectable()
    {
        if (collectables.Length == 0)
        {
            return null;
        }

        if (collectables.Length == 1)
        {
            return GameManager.GetCollectable(collectables[0].collectableType);
        }

        List<int> chances = new List<int>();
        int totalChance = 0;

        for (int i = 0; i < collectables.Length; i++)
        {
            // Sum up the total chance
            totalChance += collectables[i].spawnChance;

            // Processing the first element
            if (i == 0)
            {
                chances.Add(collectables[i].spawnChance);
            }
            // Processing elements after the first one
            else
            {
                chances.Add(collectables[i].spawnChance + chances[i - 1]);
            }
        }

        int rand = UnityEngine.Random.Range(1, totalChance + 1);

        for (int i = 0; i < chances.Count; i++)
        {
            if (i == 0)
            {
                if (rand <= chances[i])
                {
                    return GameManager.GetCollectable(collectables[i].collectableType);
                }
            }
            else
            {
                if (rand > chances[i - 1] && rand <= chances[i])
                {
                    return GameManager.GetCollectable(collectables[i].collectableType);
                }
            }
        }

        return null;
    }

    public void SpawnRandomCollectable(Vector3 position, Quaternion quaternion)
    {
        Collectable collectableToSpawn = GetRandomCollectable();

        if (collectableToSpawn == null)
        {
            return;
        }

        Instantiate(
            collectableToSpawn,
            position,
            quaternion,
            GameManager.GameSession().collectableParent
        );
    }
}
