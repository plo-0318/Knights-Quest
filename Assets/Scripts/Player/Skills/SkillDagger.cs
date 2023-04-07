using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDagger : Skill
{
    private GameObject daggerPrefab;
    private float cooldownTimer;
    private float cooldownTime;

    private readonly float BASE_DAMAGE;
    private readonly float BASE_COOLDOWN_TIME;
    private readonly float BASE_SPEED;
    private float spawnRadius;

    private int numDaggers;

    private float damage,
        speed;

    private bool piercing;

    private SoundManager soundManager;

    public SkillDagger()
    {
        name = "dagger";
        daggerPrefab = Resources.Load<GameObject>("dagger");
        type = Type.ATTACK;
        level = 1;

        BASE_DAMAGE = GameManager.GetSkillData(name).Damage;
        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).Cooldown;
        BASE_SPEED = 8f;

        cooldownTime = BASE_COOLDOWN_TIME;
        cooldownTimer = 0.5f;

        damage = BASE_DAMAGE;
        speed = BASE_SPEED;
        numDaggers = 4;
        piercing = false;

        if (GameManager.PlayerMovement().TryGetComponent<Collider2D>(out Collider2D col))
        {
            spawnRadius = Mathf.Max(col.bounds.size.x, col.bounds.size.y);
        }
        else
        {
            spawnRadius = 0.5f;
        }

        soundManager = GameManager.SoundManager();
    }

    protected override void OnLevelUp()
    {
        if (level == 2)
        {
            damage = BASE_DAMAGE * 1.25f;
            numDaggers += 2;
        }

        if (level == 3)
        {
            cooldownTime = BASE_COOLDOWN_TIME - .5f;
            numDaggers += 2;
        }

        if (level == 4)
        {
            damage = BASE_DAMAGE * 1.5f;
            numDaggers += 2;
        }

        if (level == 5)
        {
            damage = BASE_DAMAGE * 1.75f;
            cooldownTime = BASE_COOLDOWN_TIME - 1f;
            numDaggers += 2;
            piercing = true;
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

    private void Fire()
    {
        float degBetweenSpawner = 360f / numDaggers;
        float currentDeg = 0f;

        for (int i = 0; i < numDaggers; i++)
        {
            Vector2 spawnPosOffset = new Vector2(
                Mathf.Cos(currentDeg * Mathf.Deg2Rad) * spawnRadius,
                Mathf.Sin(currentDeg * Mathf.Deg2Rad) * spawnRadius
            );

            SpawnDagger(spawnPosOffset, currentDeg, spawnPosOffset.normalized);

            currentDeg += degBetweenSpawner;
        }

        soundManager.PlayClip(soundManager.audioRefs.sfxDaggerUse);

        cooldownTimer = cooldownTime;
    }

    private GameObject SpawnDagger(Vector2 offset, float zRotation, Vector2 direction)
    {
        Vector2 playerPos = GameManager.PlayerMovement().transform.position;

        GameObject spawnedDagger = GameObject.Instantiate(
            daggerPrefab,
            new Vector3(playerPos.x + offset.x, playerPos.y + offset.y, 0),
            Quaternion.identity
        );

        float baseRotation = -45f;

        spawnedDagger.transform.rotation = Quaternion.Euler(0, 0, baseRotation + zRotation);

        spawnedDagger.GetComponent<Dagger>().Init(damage, direction * speed, piercing);

        spawnedDagger.transform.parent = GameManager.GameSession().skillParent;

        return spawnedDagger;
    }
}
