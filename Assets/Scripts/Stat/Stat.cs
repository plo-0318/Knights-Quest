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

        // Modifiers created in the inspector will not have the field "statType" initialized
        // If the modifier is created in the inspector, use the field "type" to generate the stat index
        // If the modifier is created with code, use the field "statType" for the stat index
        int modifierStatIndex = newModifier.useStatType
            ? newModifier.statType
            : (int)newModifier.type;

        // Get the list of modifiers for this stat type
        HashSet<Modifier> statModifiers = modifierList[modifierStatIndex];

        // See if the modifier already exists
        Modifier existingModifier = statModifiers.FirstOrDefault(mod => mod.Equals(newModifier));

        // Already have this modifier, and replacing is false
        if (existingModifier && !replaceIfExits)
        {
            return stats[modifierStatIndex];
        }

        // If there is an exisiting modifier
        if (existingModifier)
        {
            // Check if it is timed, if it is, stop the remove coroutine
            if (existingModifier.removeModifierCoroutine != null)
            {
                GameManager.GameSession().StopCoroutine(existingModifier.removeModifierCoroutine);
            }
        }

        // Add the new modifer, if it exists, replace it
        statModifiers.Remove(existingModifier);

        statModifiers.Add(newModifier);

        // Recaculate the stat
        stats[modifierStatIndex] = CalculateStat(modifierStatIndex);

        // Start a remove timer if the modifier is timed
        if (newModifier.duration > 0)
        {
            newModifier.removeModifierCoroutine = GameManager
                .GameSession()
                .StartCoroutine(RemoveModifierAfter(newModifier, newModifier.duration));
        }

        return stats[modifierStatIndex];
    }

    public void RemoveModifier(Modifier modifier)
    {
        if (modifierList[modifier.statType].Remove(modifier))
        {
            stats[modifier.statType] = CalculateStat(modifier.statType);
        }
    }

    private IEnumerator RemoveModifierAfter(Modifier modifier, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (this != null)
        {
            RemoveModifier(modifier);
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

        if (statIndex == SPEED)
        {
            return CalculateSpeedModifier(statModifiers) * baseStat;
        }

        if (statIndex == MAX_HEALTH)
        {
            return CalculateMaxHealthModifier(statModifiers) * baseStat;
        }

        return CalculateModifierDefault(statModifiers) * baseStat;
    }

    protected float CalculateModifierDefault(HashSet<Modifier> statModifiers)
    {
        float multiplier = 1f + statModifiers.Sum(mod => mod.multiplier);

        return multiplier > 0 ? multiplier : 0.01f;
    }

    // Use the default modifier calculator but also sets the health = max health
    protected float CalculateMaxHealthModifier(HashSet<Modifier> maxHealthModifiers)
    {
        float multiplier = CalculateModifierDefault(maxHealthModifiers);

        currentHealth = multiplier * BASE_STATS[MAX_HEALTH];

        return multiplier;
    }

    // Speed multipliers of the same type shouldn't stack
    // When multiple speed modifiers are applied
    // If all positive multipliers, use the max multiplier
    // If all negative multipliers, use the min multiplier
    // If mixed multipliers (positive and negative), use the sum of max and min as multiplier
    protected float CalculateSpeedModifier(HashSet<Modifier> speedModifiers)
    {
        // No stacking speed multipliers, use the regular calculation
        if (speedModifiers.Count <= 1)
        {
            return CalculateModifierDefault(speedModifiers);
        }

        Modifier max = speedModifiers.OrderByDescending(mod => mod.multiplier).FirstOrDefault();
        Modifier min = speedModifiers.OrderBy(mod => mod.multiplier).FirstOrDefault();

        float maxMultiplier = max.multiplier;
        float minMultiplier = min.multiplier;

        // If both max and min are positive --> all multipliers are speed increase
        if (maxMultiplier > 0 && minMultiplier > 0)
        {
            return 1f + maxMultiplier;
        }

        // If both max and min are negative --> all multipliers are speed decrease
        if (maxMultiplier < 0 && minMultiplier < 0)
        {
            return 1f + minMultiplier;
        }

        // Mixed multipliers --> some speed increase multipliers and some speed decrease multipliers
        return 1f + maxMultiplier + minMultiplier;
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
