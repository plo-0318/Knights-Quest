using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHammer : Skill
{
    private GameObject hammer;
    private float cooldownTimer;
    private float cooldownTime;

    private const float BASE_DAMAGE = 1f;
    private const float BASE_COOLDOWN_TIME = 4f;

    private float damage,
        rotation, swingSpeed;
    private float lerpFactor;

    public SkillHammer()
    {
        name = "Hammer";
        hammer = Resources.Load<GameObject>("hammer");

        level = 1;

        cooldownTime = BASE_COOLDOWN_TIME;
        cooldownTimer = .5f;

        damage = BASE_DAMAGE;
        rotation = 90;
        swingSpeed = 120f;
        lerpFactor = 0.5f;
    }

    public override void Upgrade()
    {
        base.Upgrade();

        if (level == 2)
        {
            damage = BASE_DAMAGE * 1.25f;
            swingSpeed = 140f;
        }

        if (level == 3)
        {
            cooldownTime = BASE_COOLDOWN_TIME - .5f;
            rotation = 135f;
            swingSpeed = 160f;
            lerpFactor = 0.675f;
        }

        if (level == 4)
        {
            damage = BASE_DAMAGE * 1.5f;
            swingSpeed = 180f;
        }

        if (level == 5)
        {
            damage = BASE_DAMAGE * 1.75f;
            cooldownTime = BASE_COOLDOWN_TIME - 1f;
            rotation = 180f;
            lerpFactor = 1f;
            swingSpeed = 200f;
        }
    }

    public override void Use()
    {
        if (cooldownTimer <= 0)
        {
            Swing();
            cooldownTimer = cooldownTime;
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }


    //Creats the swinging and spawns hammer
    private void Swing()
    {
        float playerPosYOffset = 1.5f;
        GameObject hammerObject = SpawnHammer(new Vector2(0, playerPosYOffset), 0f);
        Hammer hammer = hammerObject.GetComponent<Hammer>();
        hammer.Init(damage, rotation, swingSpeed, false, GameManager.PlayerMovement().transform);
    }

    private GameObject SpawnHammer(Vector2 offset, float zRotation)
    {
        // Get the player position
        Vector2 playerPos = GameManager.PlayerMovement().transform.position;

        // Get the mouse position
        Vector2 mousePos = GameManager.PlayerMovement().GetMousePos();

        // Calculate the direction vector from the player to the mouse
       // Vector2 direction = Vector2.Perpendicular(mousePos - playerPos).normalized;
       // direction.Normalize();

       // Calculate the direction vector from the player to the mouse, then find the perpendicular vector
        Vector2 initialDirection = (mousePos - playerPos).normalized;
        Vector2 perpendicularDirection = -1 * Vector2.Perpendicular(initialDirection);
        Vector2 direction = Vector2.Lerp(initialDirection, perpendicularDirection, lerpFactor).normalized;

        // Calculate the spawn position as the player position plus the direction vector scaled by the offset distance
        Vector2 spawnPos = playerPos + direction * offset.magnitude;

        // Editing radius of the player
        float playerPosBaseOffset = -.2f;
        playerPos.y += playerPosBaseOffset;

        GameObject hammerParent = new GameObject("HammerMovement");
        hammerParent.transform.position = new Vector3(
            spawnPos.x,
            spawnPos.y,
            0
        );

        // Attaching the hammer parent
        HammerMovement hammerParentScript = hammerParent.AddComponent<HammerMovement>();
        hammerParentScript.SetPlayer(GameManager.PlayerMovement().gameObject);

        // Creating the hammer as a child of the parent object
        GameObject spawnedHammer = GameObject.Instantiate(
            hammer,
            Vector3.zero,
            Quaternion.identity,
            hammerParent.transform
        );

        // Set the initial rotation of the hammer based on the angle between the direction vector and the x-axis
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float baseRotation = angle - 90f;
        spawnedHammer.transform.rotation = Quaternion.Euler(0, 0, baseRotation + zRotation + 45);

        // Set the local position of the hammer to the offset distance along the direction vector
        spawnedHammer.transform.localPosition = direction * offset.magnitude;

        Rigidbody2D rb = spawnedHammer.GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        return spawnedHammer;
    }
}
