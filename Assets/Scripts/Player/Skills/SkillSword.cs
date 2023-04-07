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

    private PlayerBodyAnimatorController playerAnimatorController;

    private SoundManager soundManager;
    private List<AudioClip> SFXs;

    public SkillSword()
    {
        name = "sword";
        sword = Resources.Load<GameObject>("sword");
        type = Type.ATTACK;
        level = 1;

        BASE_DAMAGE = GameManager.GetSkillData(name).Damage;
        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).Cooldown;

        scaleMultiplier = 1f;

        cooldownTime = BASE_COOLDOWN_TIME;
        cooldownTimer = .5f;

        damage = BASE_DAMAGE;

        PlayerMovement playerMovement = GameManager.PlayerMovement();

        if (playerMovement.TryGetComponent<Collider2D>(out Collider2D col))
        {
            spawnRadius = Mathf.Max(col.bounds.size.x, col.bounds.size.y);
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

        playerAnimatorController.HandlePlayerAttack();

        cooldownTimer = cooldownTime;
    }

    private GameObject SpawnSword(float angle)
    {
        Transform player = GameManager.PlayerMovement().transform;

        Vector2 spawnOffset =
            new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad))
            * spawnRadius;

        Vector3 spawnPos = player.position + (Vector3)spawnOffset;

        GameObject spawnedSword = GameObject.Instantiate(
            sword,
            spawnPos,
            Quaternion.identity,
            player
        );

        spawnedSword.transform.rotation = Quaternion.Euler(0, 0, angle);

        spawnedSword.GetComponent<Sword>().Init(damage, spawnOffset, scaleMultiplier);

        return spawnedSword;
    }

    private void LoadSFX()
    {
        SFXs = new List<AudioClip>();

        SFXs.Add(soundManager.audioRefs.sfxSwordUse1);
        SFXs.Add(soundManager.audioRefs.sfxSwordUse2);
        SFXs.Add(soundManager.audioRefs.sfxSwordUse3);
    }
}
