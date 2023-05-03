using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBoots : Skill
{
    private Modifier speedModifier;
    private bool appliedModifier;

    public SkillBoots()
    {
        name = "boots";
        type = Type.UTILITY;

        level = 1;

        speedModifier = new Modifier(Stat.SPEED, "SkillBoots", 0.08f);

        appliedModifier = false;
    }

    protected override void OnLevelUp()
    {
        if (level == 2)
        {
            speedModifier.multiplier = 0.16f;
            appliedModifier = false;
        }

        if (level == 3)
        {
            speedModifier.multiplier = 0.24f;
            appliedModifier = false;
        }

        if (level == 4)
        {
            speedModifier.multiplier = 0.32f;
            appliedModifier = false;
        }

        if (level == 5)
        {
            speedModifier.multiplier = 0.4f;
            appliedModifier = false;
        }
    }

    public override void Use()
    {
        ApplyModifer();
    }

    private void ApplyModifer()
    {
        if (appliedModifier)
        {
            return;
        }

        PlayerStatus playerStatus = GameManager.PlayerStatus();

        playerStatus.AddModifier(speedModifier, true);

        appliedModifier = true;
    }
}
