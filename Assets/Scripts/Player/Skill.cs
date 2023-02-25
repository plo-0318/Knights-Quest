using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Skill : MonoBehaviour
{
    protected int level;

    public virtual void upgrade() { }

    public virtual void use() { }
}
