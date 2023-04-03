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
}

[System.Serializable]
public struct LevelEnemyDetail
{
    [Tooltip(
        "The amount of time, in seconds, that these enemies will spawn for (60 means these enemies will spawn for 60 seconds before moving to the next set of enemies)"
    )]
    public float spawnDuration;

    public Enemy[] enemiesToSpawn;
}
