using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCrystal : Skill
{
    private readonly float BASE_COOLDOWN_TIME;

    private float cooldownTime,
        cooldownTimer;
    private float flatHealPerentage;
    private float missingHealthHealPercetange;
    private PlayerStatus playerStatus;

    private SoundManager soundManager;

    public SkillCrystal()
    {
        name = "crystal";
        type = Type.UTILITY;

        level = 1;

        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).Cooldown;

        cooldownTimer = 0.5f;
        cooldownTime = BASE_COOLDOWN_TIME;

        flatHealPerentage = 0.025f;
        missingHealthHealPercetange = 0.05f;

        playerStatus = GameManager.PlayerStatus();
        soundManager = GameManager.SoundManager();
    }

    protected override void OnLevelUp()
    {
        if (level == 2)
        {
            flatHealPerentage = 0.03f;
            missingHealthHealPercetange = 0.075f;
        }

        if (level == 3)
        {
            flatHealPerentage = 0.035f;
            missingHealthHealPercetange = 0.1f;
        }

        if (level == 4)
        {
            flatHealPerentage = 0.04f;
            missingHealthHealPercetange = 0.125f;
        }

        if (level == 5)
        {
            flatHealPerentage = 0.045f;
            missingHealthHealPercetange = 0.15f;
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
        float missingHealth = playerStatus.GetStat(Stat.MAX_HEALTH) - playerStatus.Health;

        float missingHealthAmount = missingHealth * missingHealthHealPercetange;
        float flatAmount = flatHealPerentage * playerStatus.GetStat(Stat.MAX_HEALTH);

        playerStatus.Heal(flatAmount + missingHealthAmount);

        cooldownTimer = cooldownTime;
    }
}
