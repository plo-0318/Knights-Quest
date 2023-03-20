using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class Stat
{
    public enum Type
    {
        MAX_HEALTH,
        DAMAGE,
        SPEED,
        PROJECTILE_SPEED,
        SCALE,
    }

    private const int NUMBER_OF_STATS = 5;
    private List<float> BASE_STATS;
    private List<float> stats;
    private List<Dictionary<int, float>> modifierList;

    private float currentHealth;

    public Stat()
        : this(1f, 1f, 1f) { }

    public Stat(
        float MAX_HEALTH,
        float DAMAGE,
        float speed,
        float PROJECTILE_SPEED = 1f,
        float SCALE = 1f
    )
    {
        stats = new List<float>();

        stats.Add(MAX_HEALTH);
        stats.Add(DAMAGE);
        stats.Add(speed);
        stats.Add(PROJECTILE_SPEED);
        stats.Add(SCALE);

        BASE_STATS = new List<float>(stats);

        currentHealth = GetStat(Type.MAX_HEALTH);

        InitModifiers();
    }

    public float GetStat(Type statType)
    {
        return stats[(int)statType];
    }

    public float Health => currentHealth;

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

        return CalculateModifierDefault(statModifiers) * baseStat;

        // if (statType == Type.speed)
        // {
        //     return CalculateSpeedModifier(statModifiers) * baseStat;
        // }

        // if (statType == Type.MAX_HEALTH || statType == Type.DAMAGE)
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

    // Probably not using this
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

    public float ModifyHealth(float amount)
    {
        float newHealth = currentHealth + amount;

        if (newHealth < 0)
        {
            newHealth = 0;
        }
        else if (newHealth > GetStat(Type.MAX_HEALTH))
        {
            newHealth = GetStat(Type.MAX_HEALTH);
        }

        currentHealth = newHealth;

        return currentHealth;
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
