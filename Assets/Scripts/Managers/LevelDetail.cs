using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/LevelDetail")]
public class LevelDetail : ScriptableObject
{
    [Tooltip("Determines which enemies to spawn, and how long they will be spawning for")]
    public LevelEnemyDetail[] levelEnemyDetails;

    [Tooltip("Apply a global modifier to all the enemies in this level (Optional)")]
    public Modifier[] enemyModifiers;

    [Tooltip("The bosses of that will be spawning in this level")]
    public Enemy[] bosses;
}

[System.Serializable]
public class LevelEnemyDetail
{
    [Tooltip(
        "The amount of time, in seconds, that these enemies will spawn for (60 means these enemies will spawn for 60 seconds before moving to the next set of enemies)"
    )]
    public float spawnDuration;
    public LevelEnemy[] levelEnemies;
}

[System.Serializable]
public class LevelEnemy
{
    [Tooltip("The enemy to spawn")]
    public Enemy enemy;

    [Tooltip(
        "The proportion of this enemy type that will be spawning compared to other enemies in this list"
    )]
    [Range(1, 100)]
    public int proportion = 1;
}

public static class EnemySpawnUtil
{
    private static LevelDetail _levelDetail;
    private static List<List<Enemy>> enemyLists;
    private static List<int> enemyIndexes;
    public delegate Enemy EnemyToSpawn();

    static EnemySpawnUtil()
    {
        enemyLists = new List<List<Enemy>>();
        enemyIndexes = new List<int>();
    }

    public static void Init(LevelDetail levelDetail)
    {
        _levelDetail = levelDetail;

        foreach (LevelEnemyDetail led in _levelDetail.levelEnemyDetails)
        {
            List<Enemy> enemiesToSpawn = new List<Enemy>();

            // Adding all the enemies into the enemies to spawn list
            // Add the enemy n number of times
            // n: the propotion of the enemy
            foreach (var levelEnemy in led.levelEnemies)
            {
                int count = levelEnemy.proportion;
                Enemy enemy = levelEnemy.enemy;

                for (int i = 0; i < count; i++)
                {
                    enemiesToSpawn.Add(enemy);
                }
            }

            enemyIndexes.Add(0);
            enemyLists.Add(enemiesToSpawn);
        }
    }

    public static Enemy NextEnemyToSpawn(int enemyListIndex)
    {
        // Get the current enemy list
        List<Enemy> enemiesToSpawn = enemyLists[enemyListIndex];

        // Get the current enemy index
        int enemyIndex = enemyIndexes[enemyListIndex];

        // Get the current enemy, and increment the enemy index
        Enemy enemyToSpawn = enemiesToSpawn[enemyIndex++];

        // If index is out of bound, reset it to 0
        enemyIndex = enemyIndex >= enemiesToSpawn.Count ? 0 : enemyIndex;

        // Save the index
        enemyIndexes[enemyListIndex] = enemyIndex;

        return enemyToSpawn;
    }
}
