using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Skill
{
    public string name;

    protected int level;

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
}
