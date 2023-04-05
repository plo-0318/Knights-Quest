using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGauntlet : Skill
{
    private float multiplier;
    private Modifier itemPickupRadiusModifier;
    private PlayerStatus playerStatus;

    public SkillGauntlet()
    {
        name = "gauntlet";
        type = Type.UTILITY;

        level = 1;

        multiplier = 1f;
        itemPickupRadiusModifier = new Modifier(
            PlayerStat.ITEM_PICKUP_RADIUS,
            "SkillGauntlet",
            multiplier
        );

        playerStatus = GameManager.PlayerStatus();

        IncreaseItemPickupScale(multiplier);
    }

    public override void Upgrade()
    {
        base.Upgrade();

        if (level == 2)
        {
            multiplier += 1f;

            IncreaseItemPickupScale(multiplier);
        }

        if (level == 3)
        {
            multiplier += 1f;

            IncreaseItemPickupScale(multiplier);
        }

        if (level == 4)
        {
            multiplier += 1f;

            IncreaseItemPickupScale(multiplier);
        }

        if (level == 5)
        {
            multiplier += 1f;

            IncreaseItemPickupScale(multiplier);
        }
    }

    public override void Use() { }

    private void IncreaseItemPickupScale(float multiplier)
    {
        itemPickupRadiusModifier.multiplier = multiplier;

        playerStatus.AddModifier(itemPickupRadiusModifier);
    }
}
