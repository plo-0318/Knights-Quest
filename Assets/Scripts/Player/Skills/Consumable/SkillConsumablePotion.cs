using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillConsumablePotion : SkillConsumable
{
    public SkillConsumablePotion()
    {
        name = "consumablePotion";
        _displayName = "Health Potion";
        _description = "Recover 30% of the maximum health";
        _sprite = Util.LoadSprite("skill icons/icons", "Icons_114");
    }

    public override void Use() { }
}
