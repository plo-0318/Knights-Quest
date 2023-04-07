using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// [System.Serializable]
[CreateAssetMenu(menuName = "Skill Data")]
public class SkillData : ScriptableObject
{
    [SerializeField]
    private string displayName;

    [SerializeField]
    private string skillName;

    [SerializeField]
    private string type;

    [SerializeField]
    private string description;

    [SerializeField]
    private string lv2Effect;

    [SerializeField]
    private string lv3Effect;

    [SerializeField]
    private string lv4Effect;

    [SerializeField]
    private string lv5Effect;

    [SerializeField]
    private float damage;

    [SerializeField]
    private float cooldown;

    [SerializeField]
    private string iconPath;

    [SerializeField]
    private string iconSubName;

    [SerializeField]
    private Sprite sprite;

    public string DisplayName => displayName;
    public string SkillName => skillName;
    public string Type => type;
    public string Description => description;
    public string Lv2Effect => lv2Effect;
    public string Lv3Effect => lv3Effect;
    public string Lv4Effect => lv4Effect;
    public string Lv5Effect => lv5Effect;
    public float Damage => damage;
    public float Cooldown => cooldown;
    public string IconPath => iconPath;
    public string IconSubName => iconSubName;
    public Sprite Sprite => sprite;

    public int GetCurrentLevel()
    {
        PlayerStatus playerStatus = GameManager.PlayerStatus();

        if (playerStatus.GetSkills().TryGetValue(skillName, out Skill skill))
        {
            return skill.Level();
        }

        return 0;
    }

    public string GetCurrentDescription()
    {
        switch (GetCurrentLevel())
        {
            case 0:
                return description;
            case 1:
                return lv2Effect;
            case 2:
                return lv3Effect;
            case 3:
                return lv4Effect;
            case 4:
                return lv5Effect;
            default:
                return description;
        }
    }

    public UnityAction GetOnUISelect()
    {
        PlayerStatus playerStatus = GameManager.PlayerStatus();

        return playerStatus.OnUISkillSelect(GetSkill(skillName));
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
            case "bolt":
                return new SkillBolt();
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

    public static SkillData FromSkillConsumable(SkillConsumable skill)
    {
        SkillData consumableSkillData = CreateInstance<SkillData>();

        consumableSkillData.displayName = skill.displayName;
        consumableSkillData.skillName = skill.name;
        consumableSkillData.description = skill.description;
        consumableSkillData.sprite = skill.sprite;
        consumableSkillData.type = "CONSUMABLE";

        return consumableSkillData;
    }
}
