using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class Stat
{
    public enum StatType
    {
        MAX_HEALTH,
        DAMAGE,
        SPEED
    }

    public const int MAX_HEALTH = 0;
    public const int DAMAGE = 1;
    public const int SPEED = 2;

    protected int NUMBER_OF_STATS;

    protected List<float> BASE_STATS;
    protected List<float> stats;

    protected List<HashSet<Modifier>> modifierList;

    protected float currentHealth;

    public Stat()
        : this(1f, 1f, 1f) { }

    public Stat(float maxHealth, float damage, float speed)
    {
        NUMBER_OF_STATS = 3;

        stats = new List<float>();

        stats.Add(maxHealth);
        stats.Add(damage);
        stats.Add(speed);

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
        modifierList = new List<HashSet<Modifier>>();

        for (int i = 0; i < NUMBER_OF_STATS; i++)
        {
            modifierList.Add(new HashSet<Modifier>());
        }
    }

    public float AddModifier(Modifier newModifier, bool replaceIfExits = true)
    {
        // if new modifier is null, return
        if (!newModifier)
        {
            return 0;
        }

        // Get the list of modifiers for this stat type
        HashSet<Modifier> statModifiers = modifierList[newModifier.statType];

        // See if the modifier already exists
        Modifier existingModifier = statModifiers.FirstOrDefault(mod => mod.Equals(newModifier));

        // Already have this modifier, and replacing is false
        if (existingModifier && !replaceIfExits)
        {
            return stats[newModifier.statType];
        }

        // Add the new modifer, if it exists, replace it
        statModifiers.Remove(existingModifier);
        statModifiers.Add(newModifier);

        // Recaculate the stat
        stats[newModifier.statType] = CalculateStat(newModifier.statType);

        return stats[newModifier.statType];
    }

    public void RemoveModifier(Modifier modifier)
    {
        if (modifierList[modifier.statType].Remove(modifier))
        {
            stats[modifier.statType] = CalculateStat(modifier.statType);
        }
    }

    protected float CalculateStat(int statIndex)
    {
        HashSet<Modifier> statModifiers = modifierList[statIndex];

        float baseStat = BASE_STATS[statIndex];

        if (statModifiers.Count == 0)
        {
            return baseStat;
        }

        return CalculateModifierDefault(statModifiers) * baseStat;
    }

    protected float CalculateModifierDefault(HashSet<Modifier> statModifiers)
    {
        float multiplier = 1f + statModifiers.Sum(mod => mod.multiplier);

        return multiplier > 0 ? multiplier : 0.01f;
    }

    // Probably not using this
    // protected float CalculateSpeedModifier(Dictionary<int, float> statModifiers)
    // {
    //     float max = statModifiers.Values.Max();
    //     float min = statModifiers.Values.Min();

    //     if (max == min)
    //     {
    //         return max;
    //     }

    //     // max > 0, min > 0
    //     if (max > 0 && min > 0)
    //     {
    //         return max;
    //     }

    //     // max < 0, min < 0
    //     if (max < 0 && min < 0)
    //     {
    //         return min;
    //     }

    //     // max > 0, min < 0
    //     return max + min;
    // }

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
