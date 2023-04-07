using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGauntlet : Skill
{
    private float multiplier;
    private Modifier itemPickupRadiusModifier;
    private PlayerStatus playerStatus;

    private bool init;

    public SkillGauntlet()
    {
        name = "gauntlet";
        type = Type.UTILITY;

        level = 1;

        init = false;
        multiplier = 0.8f;

        itemPickupRadiusModifier = new Modifier(
            PlayerStat.ITEM_PICKUP_RADIUS,
            "SkillGauntlet",
            multiplier
        );

        playerStatus = GameManager.PlayerStatus();
    }

    protected override void OnLevelUp()
    {
        if (level == 2)
        {
            multiplier += 0.8f;

            IncreaseItemPickupScale(multiplier);
        }

        if (level == 3)
        {
            multiplier += 0.8f;

            IncreaseItemPickupScale(multiplier);
        }

        if (level == 4)
        {
            multiplier += 0.8f;

            IncreaseItemPickupScale(multiplier);
        }

        if (level == 5)
        {
            multiplier += 1f;

            IncreaseItemPickupScale(multiplier);
        }
    }

    public override void Use()
    {
        if (!init)
        {
            IncreaseItemPickupScale(multiplier);
            init = true;
        }
    }

    private void IncreaseItemPickupScale(float multiplier)
    {
        itemPickupRadiusModifier.multiplier = multiplier;

        playerStatus.AddModifier(itemPickupRadiusModifier);
    }
}
