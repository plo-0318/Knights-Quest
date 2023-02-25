using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private float health,
        speed,
        projectileSpeed,
        damage;

    private Skill[] skills;

    void Update()
    {
        foreach (Skill skill in skills)
        {
            skill.use();
        }
    }
}
