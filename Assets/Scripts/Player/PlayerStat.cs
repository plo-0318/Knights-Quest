using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private float health,
        speed,
        projectileSpeed,
        damage;

    private List<Skill> skills;

    private void Start()
    {
        skills = new List<Skill>();
    }

    private void Update()
    {
        foreach (Skill skill in skills)
        {
            skill.use();
        }
    }
}
