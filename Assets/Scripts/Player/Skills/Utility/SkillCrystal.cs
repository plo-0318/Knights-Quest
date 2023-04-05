using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCrystal : Skill
{
    private readonly float BASE_COOLDOWN_TIME;

    private float cooldownTime,
        cooldownTimer;
    private float healPercentage;
    private PlayerStatus playerStatus;

    private SoundManager soundManager;

    public SkillCrystal()
    {
        name = "crystal";
        type = Type.UTILITY;

        level = 1;

        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).cooldown;

        cooldownTimer = 0.5f;
        cooldownTime = BASE_COOLDOWN_TIME;

        healPercentage = 0.05f;

        playerStatus = GameManager.PlayerStatus();
        soundManager = GameManager.SoundManager();
    }

    public override void Upgrade()
    {
        base.Upgrade();

        if (level == 2)
        {
            healPercentage = 0.075f;
        }

        if (level == 3)
        {
            healPercentage = 0.1f;
        }

        if (level == 4)
        {
            healPercentage = 0.125f;
        }

        if (level == 5)
        {
            healPercentage = 0.15f;
            cooldownTime -= 2f;
        }
    }

    public override void Use()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            HealPlayer();
        }
    }

    private void HealPlayer()
    {
        float amount = healPercentage * playerStatus.GetStat(Stat.MAX_HEALTH);

        playerStatus.Heal(amount);

        cooldownTimer = cooldownTime;
    }
}
