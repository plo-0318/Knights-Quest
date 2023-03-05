using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/LevelDetail")]
public class LevelDetail : ScriptableObject
{
    public List<LevelEnemyDetail> levelEnemyDetails;
}
