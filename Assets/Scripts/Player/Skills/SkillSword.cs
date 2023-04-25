using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSword : Skill
{
    private GameObject swordPrefab,
        swordWavePrefab;
    private float cooldownTimer;
    private float cooldownTime;

    private readonly float BASE_DAMAGE;
    private readonly float BASE_COOLDOWN_TIME;
    private float spawnRadius;
    private const float SPAWN_RADIUS_OFFSET = 1.6f;

    private float damage;
    private float scaleMultiplier;

    private bool spawnSwordWave;
    private float swordWaveSpeed;

    private PlayerBodyAnimatorController playerAnimatorController;

    private SoundManager soundManager;
    private List<AudioClip> SFXs;

    public SkillSword()
    {
        name = "sword";
        swordPrefab = Resources.Load<GameObject>(name);
        swordWavePrefab = Resources.Load<GameObject>("sword wave");
        type = Type.ATTACK;
        level = 1;

        BASE_DAMAGE = GameManager.GetSkillData(name).Damage;
        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).Cooldown;

        scaleMultiplier = 1f;

        cooldownTime = BASE_COOLDOWN_TIME;
        cooldownTimer = .5f;

        damage = BASE_DAMAGE;

        spawnSwordWave = false;
        swordWaveSpeed = 8f;

        PlayerMovement playerMovement = GameManager.PlayerMovement();

        if (GameManager.PlayerMovement().PlayerCollider != null)
        {
            spawnRadius = Mathf.Max(
                GameManager.PlayerMovement().PlayerCollider.bounds.size.x,
                GameManager.PlayerMovement().PlayerCollider.bounds.size.y
            );
        }
        else
        {
            spawnRadius = 0.5f;
        }

        playerAnimatorController =
            playerMovement.GetComponentInChildren<PlayerBodyAnimatorController>();

        spawnRadius *= SPAWN_RADIUS_OFFSET;

        soundManager = GameManager.SoundManager();
        LoadSFX();
    }

    protected override void OnLevelUp()
    {
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
            scaleMultiplier = 2f;
        }

        if (level == 5)
        {
            damage = BASE_DAMAGE * 2f;

            scaleMultiplier = 2.5f;

            spawnSwordWave = true;
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
        soundManager.PlayClip(SFXs[Random.Range(1, SFXs.Count)]);

        float angle = PlayerDirectionArrow.AngleBetweenMouseAndPlayerNormalized();

        SpawnSword(angle);

        if (spawnSwordWave)
        {
            SpawnSwordWaves(angle);
        }

        playerAnimatorController.HandlePlayerAttack();

        cooldownTimer = cooldownTime;
    }

    private GameObject SpawnPrefab(float angle, GameObject prefab, Transform parent)
    {
        Transform player = GameManager.PlayerMovement().transform;

        Vector2 spawnOffset =
            new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad))
            * spawnRadius;

        Vector3 spawnPos = player.position + (Vector3)spawnOffset;

        GameObject spawnedPrefab = GameObject.Instantiate(
            prefab,
            spawnPos,
            Quaternion.identity,
            parent
        );

        spawnedPrefab.transform.rotation = Quaternion.Euler(0, 0, angle);

        return spawnedPrefab;
    }

    private void SpawnSword(float angle)
    {
        GameObject spawnedSword = SpawnPrefab(
            angle,
            swordPrefab,
            GameManager.PlayerMovement().transform
        );

        spawnedSword.GetComponent<Sword>().Init(damage, scaleMultiplier);
    }

    private void SpawnSwordWaves(float angle)
    {
        float[] angles = new float[] { angle - 25f, angle, angle + 25f };

        foreach (float spawnAngle in angles)
        {
            GameObject spanwedSwordWave = SpawnPrefab(
                spawnAngle,
                swordWavePrefab,
                GameManager.GameSession().skillParent
            );

            Vector2 direction = new Vector2(
                Mathf.Cos(spawnAngle * Mathf.Deg2Rad),
                Mathf.Sin(spawnAngle * Mathf.Deg2Rad)
            );

            spanwedSwordWave
                .GetComponent<SwordWave>()
                .Init(damage / 2f, direction * swordWaveSpeed);
        }
    }

    private void LoadSFX()
    {
        SFXs = new List<AudioClip>();

        SFXs.Add(soundManager.audioRefs.sfxSwordUse1);
        SFXs.Add(soundManager.audioRefs.sfxSwordUse2);
        SFXs.Add(soundManager.audioRefs.sfxSwordUse3);
    }
}
