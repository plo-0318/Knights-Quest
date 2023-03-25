using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class Stat
{
    public const int MAX_HEALTH = 0;
    public const int DAMAGE = 1;
    public const int SPEED = 2;
    public const int PROJECTILE_SPEED = 3;
    public const int SCALE = 4;

    protected int NUMBER_OF_STATS;
    protected List<float> BASE_STATS;
    protected List<float> stats;
    protected List<Dictionary<int, float>> modifierList;

    protected float currentHealth;

    public Stat()
        : this(1f, 1f, 1f) { }

    public Stat(
        float maxHealth,
        float damage,
        float speed,
        float prjectileSpeed = 1f,
        float scale = 1f
    )
    {
        NUMBER_OF_STATS = 5;

        stats = new List<float>();

        stats.Add(maxHealth);
        stats.Add(damage);
        stats.Add(speed);
        stats.Add(prjectileSpeed);
        stats.Add(scale);

        BASE_STATS = new List<float>(stats);

        currentHealth = GetStat(MAX_HEALTH);

        InitModifiers();
    }

    public float GetStat(int statIndex)
    {
        return stats[statIndex];
    }

    public float health => currentHealth;

    protected void InitModifiers()
    {
        modifierList = new List<Dictionary<int, float>>();

        for (int i = 0; i < NUMBER_OF_STATS; i++)
        {
            modifierList.Add(new Dictionary<int, float>());
        }
    }

    public void AddModifier(Modifier modifier, bool replaceIfExits = true)
    {
        // Get the list of modifiers for this stat type
        Dictionary<int, float> statModifiers = modifierList[modifier.statType];

        // See if the modifier already exists
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

        // Recaculate the stat
        stats[modifier.statType] = CalculateStat(modifier.statType);
    }

    public void RemoveModifier(Modifier modifier)
    {
        if (modifierList[modifier.statType].Remove(modifier.id))
        {
            stats[modifier.statType] = CalculateStat(modifier.statType);
        }
    }

    protected float CalculateStat(int statIndex)
    {
        Dictionary<int, float> statModifiers = modifierList[statIndex];
        float baseStat = BASE_STATS[statIndex];

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

    protected float CalculateModifierDefault(Dictionary<int, float> statModifiers)
    {
        float multiplier = 1f + statModifiers.Values.Sum();

        // float x = multiplier > 0 ? multiplier : 0.01f;
        // Debug.Log("\tnew multiplier = " + x);

        return multiplier > 0 ? multiplier : 0.01f;
    }

    // Probably not using this
    protected float CalculateSpeedModifier(Dictionary<int, float> statModifiers)
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
        else if (newHealth > GetStat(MAX_HEALTH))
        {
            newHealth = GetStat(MAX_HEALTH);
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
    public int statType;

    [System.NonSerialized]
    public int id;

    [Tooltip(
        "The multiplie for the stat in decimal. Enter either positive or negative value (Example: 0.25 means a increase of 25%. -0.4 means a decrease of 40%)"
    )]
    public float multiplier;

    public Modifier(int statType, int id, float multiplier)
    {
        this.statType = statType;
        this.id = id;
        this.multiplier = multiplier;
    }
}
