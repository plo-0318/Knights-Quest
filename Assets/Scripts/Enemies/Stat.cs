using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class Stat
{
    public enum Type
    {
        maxHealth,
        damage,
        speed,
        projectileSpeed,
        scale,
    }

    private const int NUMBER_OF_STATS = 5;
    private List<float> BASE_STATS;
    private List<float> stats;
    private List<Dictionary<int, float>> modifierList;

    public float health;

    public Stat()
    {
        stats = new List<float>();

        for (int i = 0; i < NUMBER_OF_STATS; i++)
        {
            stats.Add(1f);
        }

        health = stats[(int)Type.maxHealth];

        InitModifiers();
    }

    public Stat(
        float maxHealth,
        float damage,
        float speed,
        float projectileSpeed = 1f,
        float scale = 1f
    )
    {
        stats = new List<float>();

        stats.Add(maxHealth);
        stats.Add(damage);
        stats.Add(speed);
        stats.Add(projectileSpeed);
        stats.Add(scale);

        BASE_STATS = new List<float>(stats);

        health = stats[(int)Type.maxHealth];

        InitModifiers();
    }

    public float GetStat(Type statType)
    {
        return stats[(int)statType];
    }

    private void InitModifiers()
    {
        modifierList = new List<Dictionary<int, float>>();

        for (int i = 0; i < NUMBER_OF_STATS; i++)
        {
            modifierList.Add(new Dictionary<int, float>());
        }
    }

    public void AddModifier(Modifier modifier, bool replaceIfExits = true)
    {
        Dictionary<int, float> statModifiers = modifierList[(int)modifier.type];

        if (statModifiers.TryGetValue(modifier.id, out float multiplier))
        {
            if (!replaceIfExits)
            {
                return;
            }

            statModifiers[modifier.id] = modifier.multiplier;
        }
        else
        {
            statModifiers.Add(modifier.id, modifier.multiplier);
        }

        stats[(int)modifier.type] = CalculateStat(modifier.type);
    }

    public void RemoveModifier(Modifier modifier)
    {
        if (modifierList[(int)modifier.type].Remove(modifier.id))
        {
            stats[(int)modifier.type] = CalculateStat(modifier.type);
        }
    }

    private float CalculateStat(Type statType)
    {
        Dictionary<int, float> statModifiers = modifierList[(int)statType];
        float baseStat = BASE_STATS[(int)statType];

        if (statModifiers.Count == 0)
        {
            return baseStat;
        }

        // Debug.Log("new speed: " + CalculateModifierDefault(statModifiers) * baseStat);

        return CalculateModifierDefault(statModifiers) * baseStat;

        // if (statType == Type.speed)
        // {
        //     return CalculateSpeedModifier(statModifiers) * baseStat;
        // }

        // if (statType == Type.maxHealth || statType == Type.damage)
        // {
        //     return statModifiers.Values.Sum() * baseStat;
        // }

        // return baseStat;
    }

    private float CalculateModifierDefault(Dictionary<int, float> statModifiers)
    {
        float multiplier = 1f + statModifiers.Values.Sum();

        // float x = multiplier > 0 ? multiplier : 0.01f;
        // Debug.Log("\tnew multiplier = " + x);

        return multiplier > 0 ? multiplier : 0.01f;
    }

    private float CalculateSpeedModifier(Dictionary<int, float> statModifiers)
    {
        float max = statModifiers.Values.Max();
        float min = statModifiers.Values.Min();

        if (max == min)
        {
            return max;
        }

        // max > 0, min > 0
        if (max > 0 && min > 0)
        {
            return max;
        }

        // max < 0, min < 0
        if (max < 0 && min < 0)
        {
            return min;
        }

        // max > 0, min < 0
        return max + min;
    }

    public static void ApplyModifiers(Stat stat, Modifier[] modifiersToAdd)
    {
        foreach (Modifier mod in modifiersToAdd)
        {
            stat.AddModifier(mod);
        }
    }
}

[System.Serializable]
public struct Modifier
{
    public Stat.Type type;

    [System.NonSerialized]
    public int id;

    [Tooltip(
        "The multiplie for the stat in decimal. Enter either positive or negative value (Example: 0.25 means a increase of 25%. -0.4 means a decrease of 40%)"
    )]
    public float multiplier;

    public Modifier(Stat.Type type, int id, float multiplier)
    {
        this.type = type;
        this.id = id;
        this.multiplier = multiplier;
    }
}
