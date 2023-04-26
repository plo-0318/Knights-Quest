using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemySpawnUtil
{
    private static LevelDetail _levelDetail;
    private static List<List<Enemy>> enemyLists;
    private static List<int> enemyIndexes;
    public delegate Enemy EnemyToSpawn();

    private static int eliteEnemyIndex;
    private static int bossIndex;

    private static Indicator bossIndicator;

    static EnemySpawnUtil()
    {
        enemyLists = new List<List<Enemy>>();
        enemyIndexes = new List<int>();

        eliteEnemyIndex = 0;
        bossIndex = 0;

        bossIndicator = Resources.Load<Indicator>("Misc/boss indicator");
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

    public static Enemy NextEliteEnemyToSpawn()
    {
        //TODO: delete log
        Debug.Log("i: " + eliteEnemyIndex);

        if (eliteEnemyIndex >= _levelDetail.eliteEnemies.Length)
        {
            return null;
        }

        Enemy eliteEnemy = _levelDetail.eliteEnemies[eliteEnemyIndex++];

        //TODO: delete log
        Debug.Log(eliteEnemy);

        return eliteEnemy;
    }

    public static Enemy NextBossEnemyToSpawn()
    {
        if (bossIndex >= _levelDetail.bosses.Length)
        {
            return null;
        }

        Enemy boss = _levelDetail.bosses[bossIndex++];

        return boss;
    }

    public static void SpawnBossAt(Vector3 spawnPos, float blinkDuration)
    {
        Indicator indicator = GameObject.Instantiate(bossIndicator, spawnPos, Quaternion.identity);

        indicator.StartBlink(
            blinkDuration,
            () =>
            {
                GameObject.Instantiate(
                    NextBossEnemyToSpawn(),
                    spawnPos,
                    Quaternion.identity,
                    GameManager.GameSession().enemyParent
                );
            }
        );
    }
}
