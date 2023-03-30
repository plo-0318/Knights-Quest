using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBolt : Skill
{
    private GameObject bolt;
    private float cooldownTimer,
        cooldownTime;
    private readonly float BASE_DAMAGE;
    private readonly float BASE_COOLDOWN_TIME;
    private readonly float BASE_SPEED;
    private float damage,
        speed;

    public SkillBolt()
    {
        name = "bolt";
        bolt = Resources.Load<GameObject>("bolt");

        // BASE_DAMAGE = GameManager.GetSkillData(name).damage;
        // BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).cooldown;

        BASE_COOLDOWN_TIME = 2f;
        BASE_DAMAGE = 5f;
        BASE_SPEED = 10f;

        level = 1;

        damage = BASE_DAMAGE;
        speed = BASE_SPEED;
    }

    public override void Upgrade()
    {
        base.Upgrade();

        if (level == 2)
        {
            speed = BASE_SPEED * 1.25f;
        }

        if (level == 3)
        {
            cooldownTime = BASE_COOLDOWN_TIME - .5f;
        }

        if (level == 4)
        {
            speed = BASE_SPEED * 1.5f;
        }

        if (level == 5)
        {
            speed = BASE_SPEED * 2f;
            cooldownTime = BASE_COOLDOWN_TIME - 1f;
        }
    }

    public override void Use()
    {
        if (cooldownTimer <= 0)
        {
            Fire();
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public void Fire()
    {
        Vector2 playerPos = GameManager.PlayerMovement().transform.position;
        GameObject.Instantiate(bolt, playerPos, Quaternion.identity);
    }
}
