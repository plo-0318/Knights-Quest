using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillConsumable : Skill
{
    private const int NUM_SKILL_CONSUMABLES = 2;
    private const int POTION = 1;
    private const int SHIELD = 2;
    protected string _description;
    protected string _displayName;
    protected Sprite _sprite;

    public SkillConsumable()
    {
        type = Skill.Type.CONSUMABLE;
    }

    protected override void OnLevelUp() { }

    public string description => _description;
    public string displayName => _displayName;
    public Sprite sprite => _sprite;

    public static SkillConsumable GenerateRandomConsumable()
    {
        int rand = Random.Range(1, NUM_SKILL_CONSUMABLES + 1);

        // Not using SHIELD for now
        switch (rand)
        {
            case POTION:
                return new SkillConsumablePotion();
            case SHIELD:
                return new SkillConsumablePotion();
            default:
                return new SkillConsumablePotion();
        }
    }
}
