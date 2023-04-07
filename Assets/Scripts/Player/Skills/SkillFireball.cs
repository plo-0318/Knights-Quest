using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFireball : Skill
{
    private GameObject fireballPrefab;
    private GameObject fireballExplosionPrefab;
    private float cooldownTimer;
    private float cooldownTime;

    private readonly float BASE_DAMAGE;
    private readonly float BASE_COOLDOWN_TIME;

    private float damage,
        speed;
    private int numFireball;

    private bool spawning;
    private Vector2 SpawnOffset;
    private Coroutine summonFireballCoroutine;

    public SkillFireball()
    {
        name = "fireball";
        fireballPrefab = Resources.Load<GameObject>(name);
        fireballExplosionPrefab = Resources.Load<GameObject>("fireball explosion");
        type = Type.ATTACK;
        level = 1;

        BASE_DAMAGE = GameManager.GetSkillData(name).Damage;
        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).Cooldown;

        cooldownTime = BASE_COOLDOWN_TIME;
        cooldownTimer = .5f;

        damage = BASE_DAMAGE;
        speed = 6.5f;
        numFireball = 1;

        SpawnOffset = new Vector2(2f, 4f);
        spawning = false;
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

        spawning = true;

        while (numFireballToSummon > 0)
        {
            Vector3 targetPos = GetRandomPositionAroundPlayer();

            Fireball spawnedFireball = SpawnFireball(targetPos);

            Vector2 velocity = SpawnOffset.normalized * -speed;

            spawnedFireball.Init(damage, velocity, targetPos, fireballExplosionPrefab);

            numFireballToSummon--;

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

        float angle = AngleBetween(pos, spawnPos);

        spawnedFireball.transform.rotation = Quaternion.Euler(0, 0, angle + 180f);

        return spawnedFireball.GetComponent<Fireball>();
    }

    private float AngleBetween(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Atan2(pos1.y - pos2.y, pos1.x - pos2.x) * Mathf.Rad2Deg + 180f;
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
}
