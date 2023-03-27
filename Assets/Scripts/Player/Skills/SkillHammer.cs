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

    private float damage, rotation;
    private float speed = 2.0f;

    public SkillHammer()
    {
        name = "Hammer";
        hammer = Resources.Load<GameObject>("hammer");

        level = 1;

        cooldownTime = BASE_COOLDOWN_TIME;
        cooldownTimer = .5f;

        damage = BASE_DAMAGE;
        speed = 8f;
        rotation = 90;
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
            rotation = 140f;
        }

        if (level == 4)
        {
            damage = BASE_DAMAGE * 1.5f;
        }

        if (level == 5)
        {
            damage = BASE_DAMAGE * 1.75f;
            cooldownTime = BASE_COOLDOWN_TIME - 1f;
            rotation = 190f;
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

    public float getRotation(){
        return rotation;
    }

    private void Swing()
    {
        float playerPosYOffset = 1.5f;
        GameObject hammerObject = SpawnHammer(new Vector2(0, playerPosYOffset), 0f);
        Hammer hammer = hammerObject.GetComponent<Hammer>();
        hammer.Init(damage, getRotation(), speed, false, GameManager.PlayerMovement().transform);
    }

    private GameObject SpawnHammer(Vector2 offset, float zRotation)
    {
        Vector2 playerPos = GameManager.PlayerMovement().transform.position;

        float playerPosBaseOffset = -.2f;
        playerPos.y += playerPosBaseOffset;

        GameObject hammerParent = new GameObject("HammerMovement");
        hammerParent.transform.position = new Vector3(playerPos.x + offset.x, playerPos.y + offset.y, 0);

        // Attach the HammerParent script to the new parent GameObject
        HammerMovement hammerParentScript = hammerParent.AddComponent<HammerMovement>();
        hammerParentScript.SetPlayer(GameManager.PlayerMovement().gameObject);

        // Instantiate the Hammer as a child of the new parent GameObject
        GameObject spawnedHammer = GameObject.Instantiate(
            hammer,
            Vector3.zero,
            Quaternion.identity,
            hammerParent.transform
        );

        spawnedHammer.transform.localPosition = new Vector3(offset.x, offset.y, 0);

        float baseRotation = 45f;

        spawnedHammer.transform.rotation = Quaternion.Euler(0, 0, baseRotation + zRotation);

        Rigidbody2D rb = spawnedHammer.GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        return spawnedHammer;
    }
}
