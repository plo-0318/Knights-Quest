using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFireball : Skill
{
    private GameObject fireballPrefab;
    private GameObject fireballExplosionPrefab;
    private GameObject fireballFieldPrefab;
    private float cooldownTimer;
    private float cooldownTime;

    private readonly float BASE_DAMAGE;
    private readonly float BASE_COOLDOWN_TIME;

    private float damage,
        speed;
    private int numFireball;
    private bool spawnField = false;

    private bool spawning;
    private Vector2 SpawnOffset;
    private Coroutine summonFireballCoroutine;

    private SoundManager soundManager;
    private List<AudioClip> SFXs;

    public SkillFireball()
    {
        name = "fireball";
        fireballPrefab = Resources.Load<GameObject>(name);
        fireballExplosionPrefab = Resources.Load<GameObject>("fireball explosion");
        fireballFieldPrefab = Resources.Load<GameObject>("fireball field");
        type = Type.ATTACK;
        level = 1;

        BASE_DAMAGE = GameManager.GetSkillData(name).Damage;
        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).Cooldown;

        cooldownTime = BASE_COOLDOWN_TIME;
        cooldownTimer = .5f;

        damage = BASE_DAMAGE;
        speed = 7.5f;
        numFireball = 1;
        spawnField = false;

        SpawnOffset = new Vector2(2f, 4f);
        spawning = false;

        soundManager = GameManager.SoundManager();
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
            cooldownTime = BASE_COOLDOWN_TIME - 1f;
            numFireball++;
        }

        if (level == 4)
        {
            damage = BASE_DAMAGE * 1.75f;
        }

        if (level == 5)
        {
            damage = BASE_DAMAGE * 2f;
            cooldownTime = BASE_COOLDOWN_TIME - 2f;
            numFireball++;
            spawnField = true;
        }
    }

    public override void Use()
    {
        if (spawning)
        {
            return;
        }

        if (cooldownTimer <= 0)
        {
            Summon();
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void Summon()
    {
        summonFireballCoroutine = GameManager.GameSession().StartCoroutine(SummonFireballs());
    }

    private IEnumerator SummonFireballs()
    {
        int numFireballToSummon = numFireball;

        Transform player = GameManager.PlayerMovement().transform;

        spawning = true;

        while (numFireballToSummon > 0)
        {
            // Get the closest enemy
            List<Enemy> closestEnemies = GameManager.GameSession().closestEnemy(player.position);

            // Make sure the cloeset enemy reference is valid
            Enemy closestEnemy =
                closestEnemies.Count >= 1 && closestEnemies[0] != null ? closestEnemies[0] : null;

            // If closest enemy is not found or not valid, generate a random target position
            Vector3 targetPos =
                closestEnemy == null
                    ? (Vector3)GetRandomPositionAroundPlayer()
                    : closestEnemy.transform.position;

            // Spawn the fireball
            Fireball spawnedFireball = SpawnFireball(targetPos);

            // Play the fireball use sfx
            soundManager.PlayClip(SFXs[Random.Range(1, SFXs.Count)]);

            // If fireball is lv5, also spawn a burning field after explosion
            GameObject fieldPrefab = spawnField ? fireballFieldPrefab : null;

            // Initialize the fireball
            spawnedFireball.Init(
                damage,
                speed,
                closestEnemy,
                targetPos,
                fireballExplosionPrefab,
                fieldPrefab
            );

            // Decrease the number of fireballs to spawn
            numFireballToSummon--;

            // If there are more fireballs to spawn, wait 0.25 seconds
            if (numFireball > 0)
            {
                yield return new WaitForSeconds(0.25f);
            }
        }

        spawning = false;
        cooldownTimer = cooldownTime;
    }

    private Fireball SpawnFireball(Vector3 pos)
    {
        Vector3 spawnPos = pos + (Vector3)SpawnOffset;

        GameObject spawnedFireball = GameObject.Instantiate(
            fireballPrefab,
            spawnPos,
            Quaternion.identity,
            GameManager.GameSession().skillParent
        );

        return spawnedFireball.GetComponent<Fireball>();
    }

    public Vector2 GetRandomPositionAroundPlayer()
    {
        Transform player = GameManager.PlayerMovement().transform;

        float randomAngle = Random.Range(0, 361);
        float offset = 3f;

        return new Vector2(
                Mathf.Cos(randomAngle * Mathf.Deg2Rad),
                Mathf.Sin(randomAngle * Mathf.Deg2Rad)
            ) * offset
            + (Vector2)player.position;
    }

    private void LoadSFX()
    {
        SFXs = new List<AudioClip>();

        SFXs.Add(soundManager.audioRefs.sfxFireballUse1);
        SFXs.Add(soundManager.audioRefs.sfxFireballUse2);
        SFXs.Add(soundManager.audioRefs.sfxFireballUse3);
    }
}
