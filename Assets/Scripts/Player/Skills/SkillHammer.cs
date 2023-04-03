using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHammer : Skill
{
    private GameObject hammerPrefab;
    private float cooldownTimer;
    private float cooldownTime;

    private readonly float BASE_DAMAGE;
    private readonly float BASE_COOLDOWN_TIME;
    private readonly float BASE_SWING_SPEED;
    private readonly float BASE_DEGREE_TO_ROTATE;

    private float damage,
        degreeToRotate,
        swingSpeed;

    private SoundManager soundManager;
    private List<AudioClip> SFXs;

    public SkillHammer()
    {
        name = "hammer";
        hammerPrefab = Resources.Load<GameObject>("hammer");
        type = Type.ATTACK;
        level = 1;

        BASE_DAMAGE = GameManager.GetSkillData(name).damage;
        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).cooldown;
        BASE_SWING_SPEED = 350f;
        BASE_DEGREE_TO_ROTATE = 140f;

        cooldownTime = BASE_COOLDOWN_TIME;
        cooldownTimer = 0.5f;

        damage = BASE_DAMAGE;
        swingSpeed = BASE_SWING_SPEED;
        degreeToRotate = BASE_DEGREE_TO_ROTATE;

        soundManager = GameManager.SoundManager();
        LoadSFX();
    }

    public override void Upgrade()
    {
        base.Upgrade();

        if (level == 2)
        {
            damage = BASE_DAMAGE * 1.3f;
        }

        if (level == 3)
        {
            cooldownTime = BASE_COOLDOWN_TIME - .5f;
            degreeToRotate = BASE_DEGREE_TO_ROTATE * 1.3f;
            swingSpeed = BASE_SWING_SPEED * 1.15f;
        }

        if (level == 4)
        {
            damage = BASE_DAMAGE * 1.6f;
        }

        if (level == 5)
        {
            damage = BASE_DAMAGE * 2f;
            cooldownTime = BASE_COOLDOWN_TIME - 1f;
            degreeToRotate = BASE_DEGREE_TO_ROTATE * 1.6f;
            swingSpeed = BASE_SWING_SPEED * 1.25f;
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

    // Spawn and initialize the hammer
    private void Swing()
    {
        // float playerPosYOffset = 1.5f;
        // GameObject hammerObject = SpawnHammer(new Vector2(0, playerPosYOffset), 0f);
        // Hammer hammer = hammerObject.GetComponent<Hammer>();
        // hammer.Init(
        //     damage,
        //     degreeToRotate,
        //     swingSpeed,
        //     false,
        //     GameManager.PlayerMovement().transform
        // );

        // Get the normalized angle between the mouse and the player
        float angle = PlayerDirectionArrow.AngleBetweenMouseAndPlayerNormalized();

        // Create variables to store the data
        bool rotateCW = false;
        float zRotation = 0;

        Hammer spawnedHammer = SpawnHammer(angle, ref rotateCW, ref zRotation);

        soundManager.PlayClip(SFXs[Random.Range(1, SFXs.Count)]);

        spawnedHammer.Init(damage, degreeToRotate, zRotation, swingSpeed, rotateCW);
    }

    private Hammer SpawnHammer(float angle, ref bool rotateCW, ref float zRotation)
    {
        // Get the player game object transform
        Transform player = GameManager.PlayerMovement().transform;

        // Instantiate the hammer. Set its parent to the player. Get the Hammer component from its children
        GameObject hammerHolder = GameObject.Instantiate(
            hammerPrefab,
            player.position,
            Quaternion.identity,
            player
        );

        // Calculate whether to rotate left or rotate right
        // Rotate clockwise if angle is in Q1 or Q4, else rotate counter-clockwise
        rotateCW = (angle >= 0 && angle < 90f) || (angle > 270f && angle <= 360);

        // Calculate the rotation of the hammer based on the degree to rotate and the direction to rotate
        // If rotating clockwise, angle += degreeToRotate / 2, else angle -= degreeToRotate / 2
        float angleOffset = rotateCW ? degreeToRotate / 2 : degreeToRotate / -2;
        angle += angleOffset;

        // Storing the z-rotation for Hammer script to use
        zRotation = angle;

        // Rotate the spawned hammer
        hammerHolder.transform.rotation = Quaternion.Euler(
            hammerPrefab.transform.rotation.x,
            hammerPrefab.transform.rotation.y,
            angle
        );

        return hammerHolder.GetComponentInChildren<Hammer>();
    }

    private void LoadSFX()
    {
        SFXs = new List<AudioClip>();

        SFXs.Add(soundManager.audioRefs.sfxSHammerUse1);
        SFXs.Add(soundManager.audioRefs.sfxSHammerUse2);
    }

    // private GameObject SpawnHammer(Vector2 offset, float zRotation)
    // {
    //     // Get the player position
    //     Vector2 playerPos = GameManager.PlayerMovement().transform.position;

    //     // Get the mouse position
    //     Vector2 mousePos = GameManager.PlayerMovement().MousePos;

    //     // Calculate the direction vector from the player to the mouse
    //     // Vector2 direction = Vector2.Perpendicular(mousePos - playerPos).normalized;
    //     // direction.Normalize();

    //     // Calculate the direction vector from the player to the mouse, then find the perpendicular vector
    //     Vector2 initialDirection = (mousePos - playerPos).normalized;
    //     Vector2 perpendicularDirection = -1 * Vector2.Perpendicular(initialDirection);
    //     Vector2 direction = Vector2
    //         .Lerp(initialDirection, perpendicularDirection, lerpFactor)
    //         .normalized;

    //     // Calculate the spawn position as the player position plus the direction vector scaled by the offset distance
    //     Vector2 spawnPos = playerPos + direction * offset.magnitude;

    //     // Editing radius of the player
    //     float playerPosBaseOffset = -.2f;
    //     playerPos.y += playerPosBaseOffset;

    //     GameObject hammerParent = new GameObject("HammerMovement");
    //     hammerParent.transform.position = new Vector3(spawnPos.x, spawnPos.y, 0);

    //     // Attaching the hammer parent
    //     HammerMovement hammerParentScript = hammerParent.AddComponent<HammerMovement>();
    //     hammerParentScript.SetPlayer(GameManager.PlayerMovement().gameObject);

    //     // Creating the hammer as a child of the parent object
    //     GameObject spawnedHammer = GameObject.Instantiate(
    //         hammer,
    //         Vector3.zero,
    //         Quaternion.identity,
    //         hammerParent.transform
    //     );

    //     // Set the initial rotation of the hammer based on the angle between the direction vector and the x-axis
    //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //     float baseRotation = angle - 90f;
    //     spawnedHammer.transform.rotation = Quaternion.Euler(0, 0, baseRotation + zRotation + 45);

    //     // Set the local position of the hammer to the offset distance along the direction vector
    //     spawnedHammer.transform.localPosition = direction * offset.magnitude;

    //     Rigidbody2D rb = spawnedHammer.GetComponent<Rigidbody2D>();
    //     rb.isKinematic = true;

    //     return spawnedHammer;
    // }
}
