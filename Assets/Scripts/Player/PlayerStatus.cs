using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStatus : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private SoundManager soundManager;

    /////////////////////////////////////////////////////

    ////////////////////// STATS //////////////////////
    [SerializeField]
    private float baseHealth = 100f,
        baseDamage = 1f,
        baseSpeed = 6f;

    private const float DEFAULT_MOVE_SPEED_MULTIPLYER = 35f;

    private PlayerStat _stat;

    /////////////////////////////////////////////////////

    ////////////////////// SKILLS //////////////////////
    private Dictionary<string, Skill> skills;
    private const int MAX_SKILL_COUNT = 7;

    /////////////////////////////////////////////////////

    ////////////////////// STATUS //////////////////////
    private bool isDead,
        isInvincible;
    private float invincibleTime = .75f;

    /////////////////////////////////////////////////////



    private void Awake()
    {
        GameManager.RegisterPlayerStatus(this);

        _stat = new PlayerStat(baseHealth, baseDamage, baseSpeed * DEFAULT_MOVE_SPEED_MULTIPLYER);

        isDead = isInvincible = false;

        playerMovement = GetComponent<PlayerMovement>();

        skills = new Dictionary<string, Skill>();
    }

    private void Start()
    {
        soundManager = GameManager.SoundManager();
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

    public float GetStat(int statType)
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

        soundManager.PlayClip(soundManager.audioRefs.sfxPlayerHurt);

        if (knockBackDirection != Vector2.zero)
        {
            playerMovement.KnockBack(knockBackDirection);
        }

        float newHealth = _stat.ModifyHealth(-damage);

        if (newHealth <= 0)
        {
            ProcessDeath();
        }

        StartCoroutine(RecoverFromInvincible());
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
            var alpha = Mathf.PingPong(Time.time * oscillationSpeed, 1f);

            sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            currentTime += Time.deltaTime;
            yield return null;
        }

        sprite.color = originalColor;

        isInvincible = false;
    }

    private void ProcessDeath()
    {
        isDead = true;

        GameManager.GameSession().HandleGameLost();
    }

    public PlayerStat stat => _stat;

    public bool IsDead => isDead;

    // public bool HasSkill(string name)
    // {
    //     return skills.ContainsKey(name);
    // }
}
