using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Modifier
{
    public Stat.StatType type;

    [System.NonSerialized]
    public int statType;

    [System.NonSerialized]
    public string name;

    [Tooltip(
        "The multiplie for the stat in decimal. Enter either positive or negative value (Example: 0.25 means a increase of 25%. -0.4 means a decrease of 40%)"
    )]
    public float multiplier;
    public float duration;

    [System.NonSerialized]
    public bool useStatType;
    public Coroutine removeModifierCoroutine;

    public static implicit operator bool(Modifier modifier)
    {
        return modifier != null;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Modifier))
        {
            return false;
        }

        Modifier other = (Modifier)obj;
        return name == other.name;
    }

    public override int GetHashCode()
    {
        return name.GetHashCode();
    }

    public Modifier(Stat.StatType type, string name, float multiplier, float duration = 0)
    {
        this.type = type;
        this.statType = (int)type;
        this.name = name;
        this.multiplier = multiplier;
        this.duration = duration;
        this.removeModifierCoroutine = null;
        useStatType = true;
    }

    public Modifier(int type, string name, float multiplier, float duration = 0)
    {
        this.type = (Stat.StatType)Enum.ToObject(typeof(Stat.StatType), type);
        this.statType = type;
        this.name = name;
        this.multiplier = multiplier;
        this.duration = duration;
        this.removeModifierCoroutine = null;
        useStatType = true;
    }
}
