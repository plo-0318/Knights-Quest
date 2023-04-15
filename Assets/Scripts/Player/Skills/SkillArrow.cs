using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillArrow : Skill
{
    private GameObject arrowPrefab;
    private float cooldownTimer;
    private float cooldownTime;

    private readonly float BASE_DAMAGE;
    private readonly float BASE_COOLDOWN_TIME;
    private readonly float BASE_SPEED;
    private float spawnRadius;

    private float damage,
        speed;

    private Modifier speedModifier;
    private SoundManager soundManager;

    public SkillArrow()
    {
        name = "arrow";
        arrowPrefab = Resources.Load<GameObject>("arrow");
        type = Type.ATTACK;
        level = 1;

        BASE_DAMAGE = GameManager.GetSkillData(name).Damage;
        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).Cooldown;
        BASE_SPEED = 12f;

        cooldownTime = BASE_COOLDOWN_TIME;
        cooldownTimer = 0.5f;

        damage = BASE_DAMAGE;
        speed = BASE_SPEED;

        speedModifier = null;

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
    }

    protected override void OnLevelUp()
    {
        if (level == 2)
        {
            damage = BASE_DAMAGE * 1.5f;
        }

        if (level == 3)
        {
            cooldownTime = BASE_COOLDOWN_TIME - 0.5f;
        }

        if (level == 4)
        {
            damage = BASE_DAMAGE * 1.75f;
        }

        if (level == 5)
        {
            damage = BASE_DAMAGE * 2f;
            cooldownTime = BASE_COOLDOWN_TIME - 1.5f;

            speedModifier = new Modifier(Stat.StatType.SPEED, "SkillArrow", -0.75f);
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
        // Get the angle between player and mouse (normalized)
        float angle = PlayerDirectionArrow.AngleBetweenMouseAndPlayerNormalized();

        // Spawn the arrow and get its Arrow component
        Arrow spawnedArrow = SpawnArrow(angle);

        // Calculate the direction the arrow will be flying
        Vector2 direction = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        // Initialize the arrow with damage and velocity
        spawnedArrow.Init(damage, direction * speed, speedModifier);

        // Play the fire arrow sfx
        soundManager.PlayClip(soundManager.audioRefs.sfxArrowUse);

        // Reset the cooldown time
        cooldownTimer = cooldownTime;
    }

    private Arrow SpawnArrow(float angle)
    {
        // The base rotation of the arrow
        float baseRotation = 45f;

        // Calculate the spawn offset, so the arrow does not spawn directly on top of the player
        Vector2 spawnOffset = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad) * spawnRadius,
            Mathf.Sin(angle * Mathf.Deg2Rad) * spawnRadius
        );

        // Get the player position
        Vector2 playerPos = GameManager.PlayerMovement().transform.position;

        // Instantiate the arrow, and set its parent
        GameObject spawnedArrow = GameObject.Instantiate(
            arrowPrefab,
            playerPos + spawnOffset,
            Quaternion.identity,
            GameManager.GameSession().skillParent
        );

        // Rotate the arrow to face the right direction
        spawnedArrow.transform.rotation = Quaternion.Euler(0, 0, angle - baseRotation);

        // Return the Arrow component
        return spawnedArrow.GetComponent<Arrow>();
    }
}
