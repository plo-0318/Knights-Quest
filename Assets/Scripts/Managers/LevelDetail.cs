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
    public Enemy[] eliteEnemies;
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
