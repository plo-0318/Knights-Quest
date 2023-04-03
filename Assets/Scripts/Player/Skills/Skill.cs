using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public enum Type
    {
        ATTACK,
        UTILITY,
        CONSUMABLE
    }

    public string name;

    protected int level;

    protected Type type;

    public static implicit operator bool(Skill skill)
    {
        return skill != null;
    }

    public static bool operator ==(Skill skill1, Skill skill2)
    {
        if (ReferenceEquals(skill1, skill2))
            return true;
        if (skill1 is null || skill2 is null)
            return false;
        return skill1.name == skill2.name;
    }

    public static bool operator !=(Skill skill1, Skill skill2)
    {
        return !(skill1 == skill2);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Skill other = (Skill)obj;
        return name == other.name;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + this.name.GetHashCode();
        return hash;
    }

    public virtual void Upgrade()
    {
        if (level >= 5)
        {
            return;
        }

        level++;
    }

    public virtual void Use() { }

    public int Level()
    {
        return level;
    }

    public Type SkillType => type;
}
