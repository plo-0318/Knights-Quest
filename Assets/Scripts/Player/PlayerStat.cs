using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField]
    private float baseHealth = 100f,
        baseDamage = 1f,
        baseSpeed = 5f,
        baseProjectileSpeed = 1f;

    private const float DEFAULT_MOVE_SPEED_MULTIPLYER = 35f;

    private Stat _stat;

    private Dictionary<string, Skill> skills;
    private const int NUM_SKILLS = 10;

    private void Awake()
    {
        GameManager.RegisterPlayerStat(this);

        _stat = new Stat(
            baseHealth,
            baseDamage,
            baseSpeed * DEFAULT_MOVE_SPEED_MULTIPLYER,
            baseProjectileSpeed
        );
    }

    private void Start()
    {
        skills = new Dictionary<string, Skill>();
    }

    private void Update()
    {
        foreach (KeyValuePair<string, Skill> kvp in skills)
        {
            kvp.Value.Use();
        }
    }

    public void AssignSkill(Skill skillToAdd)
    {
        // If player has not learned this skill, add it
        if (!skills.TryGetValue(skillToAdd.name, out Skill skill))
        {
            skills.Add(skillToAdd.name, skillToAdd);
        }
        // else level up this skill
        else
        {
            skill.Upgrade();
        }
    }

    public Dictionary<string, Skill> GetSkills()
    {
        return skills;
    }

    public Stat stat => _stat;

    // public bool HasSkill(string name)
    // {
    //     return skills.ContainsKey(name);
    // }
}
