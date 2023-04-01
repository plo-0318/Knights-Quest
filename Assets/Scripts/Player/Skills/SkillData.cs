using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillData
{
    public string displayName;
    public string name;
    public string type;
    public string description;
    public string lv2Effect;
    public string lv3Effect;
    public string lv4Effect;
    public string lv5Effect;
    public float damage;
    public float cooldown;
    public string iconPath;
    public string iconSubName;
    public Sprite sprite;

    public SkillData(SkillData other)
    {
        displayName = other.displayName;
        name = other.name;
        type = other.type;
        description = other.description;
        lv2Effect = other.lv2Effect;
        lv3Effect = other.lv3Effect;
        lv4Effect = other.lv4Effect;
        lv5Effect = other.lv5Effect;
        damage = other.damage;
        cooldown = other.cooldown;
        iconPath = other.iconPath;
        iconSubName = other.iconSubName;
        sprite = other.sprite;
    }

    public static Skill GetSkill(string name)
    {
        switch (name)
        {
            case "sword":
                return new SkillSword();
            case "dagger":
                return new SkillDagger();
            case "field":
                return new SkillField();
            case "arrow":
                return new SkillArrow();
            case "fireball":
                return new SkillFireball();
            case "hammer":
                return new SkillHammer();
            //TODO: Implement this after get bolt script
            // case "bolt":
            //     return new SkillBolt();
            case "boots":
                return new SkillBoots();
            case "gauntlet":
                return new SkillGauntlet();
            case "crystal":
                return new SkillCrystal();
            case "shield":
                return new SkillShield();
            default:
                return null;
        }
    }

    public static List<Skill> GetSkills(IEnumerable<string> names)
    {
        List<Skill> skills = new List<Skill>();

        foreach (string name in names)
        {
            skills.Add(GetSkill(name));
        }

        return skills;
    }
}
