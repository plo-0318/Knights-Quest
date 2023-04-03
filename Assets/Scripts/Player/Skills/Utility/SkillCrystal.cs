using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCrystal : Skill
{
    private readonly float BASE_COOLDOWN_TIME;

    public SkillCrystal()
    {
        name = "crystal";

        level = 1;

        type = Type.UTILITY;

        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).cooldown;
    }

    public override void Upgrade()
    {
        base.Upgrade();

        if (level == 2) { }

        if (level == 3) { }

        if (level == 4) { }

        if (level == 5) { }
    }

    public override void Use() { }
}
