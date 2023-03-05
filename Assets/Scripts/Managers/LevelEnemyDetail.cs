using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/LevelEnemyDetail")]
public class LevelEnemyDetail : ScriptableObject
{
    [Tooltip(
        "The amount of time, in seconds, that these enemies will spawn for (60 means these enemies will spawn for 60 seconds before moving to the next set of enemies)"
    )]
    public float spawnDuration = 60f;

    public List<Enemy> enemiesToSpawn;
}
