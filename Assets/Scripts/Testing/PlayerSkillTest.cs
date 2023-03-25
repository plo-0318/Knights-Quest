using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillTest : MonoBehaviour
{
    public void TEST_AddDaggerSkill()
    {
        GameManager.PlayerStatus().AssignSkill(new SkillDagger());
    }

    public void TEST_AddFireballSkill()
    {
        GameManager.PlayerStatus().AssignSkill(new SkillFireball());
    }

    public void TEST_AddArrowSkill()
    {
        GameManager.PlayerStatus().AssignSkill(new SkillArrow());
    }

    public void TEST_AddHammerSkill()
    {
        GameManager.PlayerStatus().AssignSkill(new SkillHammer());
    }
}
