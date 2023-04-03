using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGauntlet : Skill
{
    public SkillGauntlet()
    {
        name = "gauntlet";

        level = 1;

        type = Type.UTILITY;
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
