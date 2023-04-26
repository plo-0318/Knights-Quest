using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBolt : Skill
{
    private GameObject boltPrefab;
    private GameObject boltOnHitPrefab;
    private BoltBurst boltBurstPrefab;
    private float cooldownTimer;
    private float cooldownTime;

    private readonly float BASE_DAMAGE;
    private readonly float BASE_COOLDOWN_TIME;
    private readonly float BASE_SPEED;
    private float spawnRadius;

    private float damage,
        speed;

    private Transform player;

    private SoundManager soundManager;
    private List<AudioClip> SFXs;
    private AudioClip enemyOnHitSFX;

    public SkillBolt()
    {
        name = "bolt";
        boltPrefab = Resources.Load<GameObject>("bolt");
        boltOnHitPrefab = Resources.Load<GameObject>("bolt on-hit");
        boltBurstPrefab = Resources.Load<BoltBurst>("bolt burst");
        type = Type.ATTACK;
        level = 1;

        BASE_DAMAGE = GameManager.GetSkillData(name).Damage;
        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).Cooldown;
        BASE_SPEED = 10f;

        cooldownTime = BASE_COOLDOWN_TIME;
        cooldownTimer = 0.5f;

        damage = BASE_DAMAGE;
        speed = BASE_SPEED;

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

        soundManager = GameManager.SoundManager();
        player = GameManager.PlayerMovement().transform;

        LoadSFX();
    }

    protected override void OnLevelUp()
    {
        if (level == 2)
        {
            damage = BASE_DAMAGE * 1.25f;
        }

        if (level == 3)
        {
            damage = BASE_DAMAGE * 1.5f;
            cooldownTime -= 0.1f;
        }

        if (level == 4)
        {
            damage = BASE_DAMAGE * 1.75f;
            cooldownTime -= 0.3f;
        }

        if (level == 5)
        {
            damage = BASE_DAMAGE * 2f;
            cooldownTime -= 0.1f;
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
        List<Vector3> closestEnemy = GameManager
            .GameSession()
            .closestEnemyPosition(player.position);

        float angle =
            closestEnemy.Count > 0
                ? AngleBetweenPosAndPlayer(closestEnemy[0])
                : PlayerDirectionArrow.AngleBetweenMouseAndPlayerNormalized();

        Vector2 direction = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad) * spawnRadius,
            Mathf.Sin(angle * Mathf.Deg2Rad) * spawnRadius
        );

        Bolt spawnedBolt = SpawnBolt(direction, angle);

        if (spawnedBolt != null)
        {
            spawnedBolt.Init(
                damage,
                speed,
                direction.normalized,
                boltOnHitPrefab,
                enemyOnHitSFX,
                level,
                boltBurstPrefab
            );
            soundManager.PlayClip(SFXs[Random.Range(1, SFXs.Count)]);
        }

        cooldownTimer = cooldownTime;
    }

    private Bolt SpawnBolt(Vector3 spawnOffset, float angle)
    {
        // Instantiate the bolt. Set its parent to skill holder
        GameObject bolt = GameObject.Instantiate(
            boltPrefab,
            player.position + spawnOffset,
            Quaternion.identity,
            GameManager.GameSession().skillParent
        );

        // Rotate the bolt to face the right direction
        bolt.transform.rotation = Quaternion.Euler(0, 0, angle);

        return bolt.GetComponent<Bolt>();
    }

    private float AngleBetweenPosAndPlayer(Vector3 pos)
    {
        Vector3 playerPos = GameManager.PlayerMovement().transform.position;

        return Mathf.Atan2(playerPos.y - pos.y, playerPos.x - pos.x) * Mathf.Rad2Deg + 180f;
    }

    private void LoadSFX()
    {
        SFXs = new List<AudioClip>();

        SFXs.Add(soundManager.audioRefs.sfxBoltUse1);
        SFXs.Add(soundManager.audioRefs.sfxBoltUse2);
        SFXs.Add(soundManager.audioRefs.sfxBoltUse3);

        enemyOnHitSFX = soundManager.audioRefs.sfxEnemyHurtBolt;
    }
}
