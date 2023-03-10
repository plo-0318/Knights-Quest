using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRepellentField : MonoBehaviour
{
    private GameObject repellent;
    private float cooldownTimer;
    private float cooldownTime;

    private const float BASE_DAMAGE = 1f;
    private const float BASE_COOLDOWN_TIME = 3f;

    private float damage,
        speed;

    public SkillRepellentField()
    {
        name = "RepellentField";
        repellent = Resources.Load<GameObject>("repellentField");

        level = 1;

        cooldownTime = BASE_COOLDOWN_TIME;
        cooldownTimer = .5f;

        damage = BASE_DAMAGE;
        speed = 8f;
    }

    public override void Upgrade()
    {
        base.Upgrade();

        if (level == 2)
        {
            damage = BASE_DAMAGE * 1.25f;
        }

        if (level == 3)
        {
            cooldownTime = BASE_COOLDOWN_TIME - .5f;
        }

        if (level == 4)
        {
            damage = BASE_DAMAGE * 1.5f;
        }

        if (level == 5)
        {
            damage = BASE_DAMAGE * 1.75f;
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

    private void Fire()
    {
        float playerPosXOffset = .6f,
            playerPosYOffset = .6f;

        SpawnRepellentField(new Vector2(0, playerPosYOffset), 0, Vector2.up);
        SpawnRepellentField(new Vector2(-playerPosXOffset, 0), 90, Vector2.left);
        SpawnRepellentField(new Vector2(0, -playerPosYOffset), 180, Vector2.down);
        SpawnRepellentField(new Vector2(playerPosXOffset, 0), 270, Vector2.right);

        if (level == 5)
        {
            // Top left
            SpawnRepellentField(
                new Vector2(-playerPosXOffset, playerPosYOffset),
                45,
                (Vector2.up + Vector2.left).normalized
            );
            // Bottom left
            SpawnRepellentField(
                new Vector2(-playerPosXOffset, -playerPosYOffset),
                135,
                (Vector2.down + Vector2.left).normalized
            );
            // Bottom right
            SpawnRepellentField(
                new Vector2(playerPosXOffset, -playerPosYOffset),
                225,
                (Vector2.down + Vector2.right).normalized
            );
            // Top right
            SpawnRepellentField(
                new Vector2(playerPosXOffset, playerPosYOffset),
                315,
                (Vector2.up + Vector2.right).normalized
            );
        }

        cooldownTimer = cooldownTime;
    }

    private GameObject SpawnRepellentField(Vector2 offset, float zRotation, Vector2 velocity)
    {
        Vector2 playerPos = GameManager.PlayerMovement().transform.position;

        float playerPosBaseOffset = -.2f;
        playerPos.y += playerPosBaseOffset;

        GameObject spawnedRepellentField = GameObject.Instantiate(
            repellent,
            new Vector3(playerPos.x + offset.x, playerPos.y + offset.y, 0),
            Quaternion.identity
        );

        float baseRotation = 45f;

        spawnedRepellentField.transform.rotation = Quaternion.Euler(0, 0, baseRotation + zRotation);

        spawnedRepellentField.GetComponent<RepellentField>().Init(damage, velocity * speed);

        return spawnedRepellentField;
    }
}
