using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/GameLevelDetail")]
public class GameLevelDetail : ScriptableObject
{
    [SerializeField]
    private LevelDetail noob;

    [SerializeField]
    private LevelDetail easy;

    [SerializeField]
    private LevelDetail normal;

    [SerializeField]
    private LevelDetail hard;

    public LevelDetail GetCurrentLevelDetail()
    {
        string currentDifficulty = PlayerPrefsController.GetDifficulty();

        switch (currentDifficulty)
        {
            case "noob":
                return noob;
            case "easy":
                return easy;
            case "normal":
                return normal;
            case "hard":
                return hard;
            default:
                return normal;
        }
    }
}
