using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSword : Skill
{
    private GameObject sword;
    private float cooldownTimer;
    private float cooldownTime;

    private readonly float BASE_DAMAGE;
    private readonly float BASE_COOLDOWN_TIME;
    private float spawnRadius;
    private const float SPAWN_RADIUS_OFFSET = 2f;

    private float damage;
    private float scaleMultiplier;

    private SoundManager soundManager;

    public SkillSword()
    {
        name = "sword";
        sword = Resources.Load<GameObject>("sword");

        level = 1;

        BASE_DAMAGE = GameManager.GetSkillData(name).damage;
        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).cooldown;

        scaleMultiplier = 1f;

        cooldownTime = BASE_COOLDOWN_TIME;
        cooldownTimer = .5f;

        damage = BASE_DAMAGE;

        if (GameManager.PlayerMovement().TryGetComponent<Collider2D>(out Collider2D col))
        {
            spawnRadius = Mathf.Max(col.bounds.size.x, col.bounds.size.y);
        }
        else
        {
            spawnRadius = 0.5f;
        }

        spawnRadius *= SPAWN_RADIUS_OFFSET;

        soundManager = GameManager.SoundManager();
    }

    public override void Upgrade()
    {
        base.Upgrade();

        if (level == 2)
        {
            damage = BASE_DAMAGE * 1.25f;
            cooldownTime -= 0.25f;
        }

        if (level == 3)
        {
            scaleMultiplier = 1.5f;
        }

        if (level == 4)
        {
            damage = BASE_DAMAGE * 1.5f;
            cooldownTime -= 0.25f;
        }

        if (level == 5)
        {
            damage = BASE_DAMAGE * 2f;

            scaleMultiplier = 2f;
        }
    }

    public override void Use()
    {
        if (cooldownTimer <= 0)
        {
            SwingSword();
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void SwingSword()
    {
        soundManager.PlayClip(soundManager.audioRefs.sfxSwordUse);

        float angle = PlayerDirectionArrow.AngleBetweenMouseAndPlayerNormalized();

        SpawnSword(angle);

        cooldownTimer = cooldownTime;
    }

    private GameObject SpawnSword(float angle)
    {
        Vector2 playerPos = GameManager.PlayerMovement().transform.position;

        Vector2 spawnOffset =
            new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad))
            * spawnRadius;

        Vector3 spawnPos = playerPos + spawnOffset;

        GameObject spawnedSword = GameObject.Instantiate(sword, spawnPos, Quaternion.identity);

        spawnedSword.transform.rotation = Quaternion.Euler(0, 0, angle);

        spawnedSword.GetComponent<Sword>().Init(damage, spawnOffset, scaleMultiplier);

        spawnedSword.transform.parent = GameManager.GameSession().skillParents;

        return spawnedSword;
    }
}
