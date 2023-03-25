using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillTest : MonoBehaviour
{
    public void TEST_AddDaggerSkill()
    {
        GameManager.PlayerStatus().AssignSkill(new SkillDagger());
    }
}
