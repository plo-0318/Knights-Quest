using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillConsumableShield : SkillConsumable
{
    public SkillConsumableShield()
    {
        name = "consumableShield";
        _displayName = "Knight Shield";
        _description = "Gain a temporary shield that blocks the first incoming damage";
        _sprite = Util.LoadSprite("skill icons/icons", "Icons_64");
    }

    public override void Use() { }
}
