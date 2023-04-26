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
    private bool hammerPersist;
    private bool stopSpawning;

    private Transform player;
    private SoundManager soundManager;
    private List<AudioClip> SFXs;

    public SkillHammer()
    {
        name = "hammer";
        hammerPrefab = Resources.Load<GameObject>("hammer");
        type = Type.ATTACK;
        level = 1;

        BASE_DAMAGE = GameManager.GetSkillData(name).Damage;
        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).Cooldown;
        BASE_SWING_SPEED = 350f;
        BASE_DEGREE_TO_ROTATE = 140f;

        cooldownTime = BASE_COOLDOWN_TIME;
        cooldownTimer = 0.5f;

        damage = BASE_DAMAGE;
        swingSpeed = BASE_SWING_SPEED;
        degreeToRotate = BASE_DEGREE_TO_ROTATE;
        hammerPersist = stopSpawning = false;

        player = GameManager.PlayerMovement().transform;
        soundManager = GameManager.SoundManager();
        LoadSFX();
    }

    protected override void OnLevelUp()
    {
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

            hammerPersist = true;
        }
    }

    public override void Use()
    {
        if (stopSpawning)
        {
            return;
        }

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
        if (hammerPersist)
        {
            stopSpawning = true;
            Hammer.RemoveAllSpawnedHammers();

            swingSpeed = BASE_SWING_SPEED;
            damage = BASE_DAMAGE * 1.6f;
        }

        // Get the closest enemy positions
        List<Vector3> closestEnemy = GameManager
            .GameSession()
            .closestEnemyPosition(player.position);

        // If there is at least 1 enemy, use the closest enemies position
        // Else use the mouse position
        float angle =
            closestEnemy.Count > 0
                ? Util.GetNormalizedAngle(player.position, closestEnemy[0])
                : PlayerDirectionArrow.AngleBetweenMouseAndPlayerNormalized();

        // Create variables to store the data
        bool rotateCW = false;
        float zRotation = 0;

        Hammer spawnedHammer = SpawnHammer(angle, ref rotateCW, ref zRotation);

        spawnedHammer.Init(damage, degreeToRotate, zRotation, swingSpeed, rotateCW, hammerPersist);

        soundManager.PlayClip(SFXs[Random.Range(1, SFXs.Count)]);
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

        SFXs.Add(soundManager.audioRefs.sfxHammerUse1);
        SFXs.Add(soundManager.audioRefs.sfxHammerUse2);
    }
}
