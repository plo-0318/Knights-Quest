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

    [Tooltip("The elite enemies of that will be spawning in this level")]
    public EnemySpawnEvent[] eliteEnemyEvents;

    [Tooltip(
        "Events that trigger at certain time which change the number of enemies that will be spawned"
    )]
    public EnemySpawnerEvent[] enemySpawnerEvents;
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

[System.Serializable]
public class EnemySpawnEvent
{
    [Tooltip("The time that this event will happen")]
    public int time;

    [Tooltip("The enemy that will be spawned at this time")]
    public Enemy enemy;

    public TimedEvent ToTimedEvent()
    {
        return new TimedEvent(time, GameManager.SpawnerManager().SpawnEliteEnemy);
    }
}

[System.Serializable]
public class EnemySpawnerEvent
{
    [Tooltip("The time that this event will happen")]
    public int time;

    [Tooltip("The maximum number of enemies allowed per wave")]
    public int maxEnemyPerWave;

    [Tooltip("The maximum total number of enemies allowed at any given point")]
    public int maxEnemyTotal;

    [Tooltip("The number of spawners around the enemy")]
    public int numSpawners;

    public TimedEvent ToTimedEvent(Func<int, int, int, Action> toCallBack)
    {
        return new TimedEvent(time, toCallBack(maxEnemyPerWave, maxEnemyTotal, numSpawners));
    }
}
