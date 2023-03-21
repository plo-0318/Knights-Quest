using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStat : MonoBehaviour
{
    ////////////////////// REFS //////////////////////
    private PlayerMovement playerMovement;

    /////////////////////////////////////////////////////

    ////////////////////// STATS //////////////////////
    [SerializeField]
    private float baseHealth = 100f,
        baseDamage = 1f,
        baseSpeed = 5f,
        baseProjectileSpeed = 1f;

    private const float DEFAULT_MOVE_SPEED_MULTIPLYER = 35f;

    private Stat _stat;
    private float itemPickupScale;
    private int killCount;
    private double exp;

    /////////////////////////////////////////////////////

    ////////////////////// SKILLS //////////////////////
    private Dictionary<string, Skill> skills;
    private const int MAX_SKILL_COUNT = 7;

    /////////////////////////////////////////////////////

    ////////////////////// STATUS //////////////////////
    private bool isDead,
        isInvincible;
    private float invincibleTime = .75f;

    public event Action onPlayerDeath;

    /////////////////////////////////////////////////////

    ////////////////////// EXP //////////////////////
    private const int BASE_EXP = 100;
    private const int LINEAR_INCREMENT = 50;
    private const int EXPONENTIAL_INCREMENT = 25;

    /////////////////////////////////////////////////////

    private void Awake()
    {
        GameManager.RegisterPlayerStat(this);

        _stat = new Stat(
            baseHealth,
            baseDamage,
            baseSpeed * DEFAULT_MOVE_SPEED_MULTIPLYER,
            baseProjectileSpeed
        );

        itemPickupScale = 1f;
        killCount = 0;
        exp = 0;

        isDead = isInvincible = false;

        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        skills = new Dictionary<string, Skill>();
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        foreach (KeyValuePair<string, Skill> kvp in skills)
        {
            kvp.Value.Use();
        }
    }

    public void AssignSkill(Skill skillToAdd)
    {
        // If player has not learned this skill, add it
        if (!skills.TryGetValue(skillToAdd.name, out Skill skill))
        {
            skills.Add(skillToAdd.name, skillToAdd);
        }
        // else level up this skill
        else
        {
            skill.Upgrade();
        }
    }

    public Dictionary<string, Skill> GetSkills()
    {
        return skills;
    }

    public Stat stat => _stat;

    public float GetStat(Stat.Type statType)
    {
        return _stat.GetStat(statType);
    }

    public void Hurt(float damage)
    {
        Hurt(damage, Vector2.zero);
    }

    public void Hurt(float damage, Vector2 knockBackDirection)
    {
        if (isInvincible)
        {
            return;
        }

        isInvincible = true;

        if (knockBackDirection != Vector2.zero)
        {
            playerMovement.KnockBack(knockBackDirection);
        }

        float newHealth = stat.ModifyHealth(-damage);

        if (newHealth <= 0)
        {
            ProcessDeath();
        }

        StartCoroutine(RecoverFromInvincible());
    }

    public void IncrementKillCount()
    {
        killCount++;
    }

    private IEnumerator RecoverFromInvincible()
    {
        // yield return new WaitForSeconds(invincibleTime);

        // Blink
        float currentTime = 0;
        float oscillationSpeed = 6f;

        var sprite = GetComponent<SpriteRenderer>();
        Color originalColor = new Color(
            sprite.color.r,
            sprite.color.g,
            sprite.color.b,
            sprite.color.a
        );

        while (currentTime < invincibleTime)
        {
            var a = Mathf.PingPong(Time.time * oscillationSpeed, 1f);

            sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, a);

            currentTime += Time.deltaTime;
            yield return null;
        }

        sprite.color = originalColor;

        isInvincible = false;
    }

    private void ProcessDeath()
    {
        isDead = true;
        onPlayerDeath?.Invoke();
    }

    private double ExpNeededToLevelUp(int level)
    {
        return BASE_EXP + LINEAR_INCREMENT * level + EXPONENTIAL_INCREMENT * Mathf.Pow(level, 2);
    }

    public int KillCount => killCount;
    public bool IsDead => isDead;

    // public bool HasSkill(string name)
    // {
    //     return skills.ContainsKey(name);
    // }
}
