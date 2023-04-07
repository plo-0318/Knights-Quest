using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShield : Skill
{
    private readonly float BASE_COOLDOWN_TIME;
    private Shield shieldPrefab;
    private Shield shieldGameObject;
    private bool blocksTwice;
    private float cooldownTime,
        cooldownTimer;
    private SoundManager soundManager;

    public SkillShield()
    {
        name = "shield";
        shieldPrefab = Resources.Load<Shield>("shield_bubble");
        type = Type.UTILITY;

        level = 1;

        blocksTwice = false;

        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).Cooldown;

        cooldownTimer = 0.5f;
        cooldownTime = BASE_COOLDOWN_TIME;

        shieldGameObject = null;
        soundManager = GameManager.SoundManager();
    }

    protected override void OnLevelUp()
    {
        if (level == 2)
        {
            cooldownTime--;
        }

        if (level == 3)
        {
            cooldownTime--;
        }

        if (level == 4)
        {
            cooldownTime--;
        }

        if (level == 5)
        {
            cooldownTime--;
            blocksTwice = true;

            if (shieldGameObject != null && !shieldGameObject.IsBreaking)
            {
                shieldGameObject.Init(blocksTwice);
            }
        }
    }

    public override void Use()
    {
        if (shieldGameObject != null)
        {
            return;
        }

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            SpawnShield();
        }
    }

    public void SpawnShield()
    {
        // Has a shield
        if (shieldGameObject != null)
        {
            return;
        }

        // Spawn a shield and attach it to the player
        Transform player = GameManager.PlayerMovement().transform;

        shieldGameObject = GameObject.Instantiate(
            shieldPrefab,
            player.position,
            Quaternion.identity,
            player
        );

        shieldGameObject.Init(blocksTwice);

        cooldownTimer = cooldownTime;
    }

    public bool BreakShield()
    {
        // No shield found
        if (shieldGameObject == null)
        {
            return false;
        }

        // The shield is already breaking
        if (shieldGameObject.IsBreaking)
        {
            return false;
        }

        // Start breaking the shield
        shieldGameObject.Break();

        soundManager.PlayClip(soundManager.audioRefs.sfxPlayerShieldBreak);

        return true;
    }
}
