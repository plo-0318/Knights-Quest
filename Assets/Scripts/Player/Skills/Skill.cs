using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Skill
{
    public string name;

    protected int level;

    public virtual void upgrade()
    {
        if (level >= 5)
        {
            return;
        }

        level++;
    }

    public virtual void use() { }

    public int Level()
    {
        return level;
    }
}
