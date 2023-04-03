using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GemSpawn
{
    public enum GemType
    {
        GREEN,
        BLUE,
        ORANGE,
        RED
    }

    [SerializeField]
    [Tooltip("The type of gem to be spawned")]
    private GemType gem;

    [SerializeField]
    [Tooltip(
        "The chance of this type of gem spawning. This field only matters if there are more than 1 gems. The chance of spawning will be calculated based on the total chance."
    )]
    [Range(1, 100)]
    private int spawnChance = 1;

    public GemType Gem => gem;
    public int SpawnChance => spawnChance;

    private static CollectableGem LoadGem(GemType type)
    {
        string gemName = "";

        switch (type)
        {
            case GemType.GREEN:
                gemName = "green exp gem";
                break;
            case GemType.BLUE:
                gemName = "blue exp gem";
                break;
            case GemType.ORANGE:
                gemName = "orange exp gem";
                break;
            case GemType.RED:
                gemName = "red exp gem";
                break;
            default:
                gemName = "green exp gem";
                break;
        }

        CollectableGem gem = Resources.Load<CollectableGem>("collectables/" + gemName);

        return gem;
    }

    public static CollectableGem GetGem(GemSpawn[] gemSpawns)
    {
        if (gemSpawns.Length == 0)
        {
            return null;
        }

        if (gemSpawns.Length == 1)
        {
            return LoadGem(gemSpawns[0].Gem);
        }

        List<int> chances = new List<int>();
        int totalChance = 0;

        for (int i = 0; i < gemSpawns.Length; i++)
        {
            // Sum up the total chance
            totalChance += gemSpawns[i].SpawnChance;

            // Processing the first element
            if (i == 0)
            {
                chances.Add(gemSpawns[i].SpawnChance);
            }
            // Processing elements after the first one
            else
            {
                chances.Add(gemSpawns[i].spawnChance + chances[i - 1]);
            }
        }

        //TODO: DELETE log
        // string s = "";
        // foreach (int i in chances)
        // {
        //     s += i.ToString() + " ";
        // }
        // Debug.Log("chances = [" + s.Trim() + "]");

        int rand = UnityEngine.Random.Range(1, totalChance + 1);

        // Debug.Log("rand = " + rand);

        for (int i = 0; i < chances.Count; i++)
        {
            if (i == 0)
            {
                if (rand <= chances[i])
                {
                    // Debug.Log("i = 0 --> match returning " + gemSpawns[i].Gem.ToString() + " gem");

                    return LoadGem(gemSpawns[i].Gem);
                }
            }
            else
            {
                if (rand > chances[i - 1] && rand <= chances[i])
                {
                    // Debug.Log(
                    //     "i = " + i + " --> match returning " + gemSpawns[i].Gem.ToString() + " gem"
                    // );

                    return LoadGem(gemSpawns[i].Gem);
                }
            }
        }

        return null;
    }
}
