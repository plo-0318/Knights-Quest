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
    private float itemPickupScale,
        killCount,
        exp;

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
        killCount = exp = 0;

        isDead = isInvincible = false;

        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        skills = new Dictionary<string, Skill>();

        // StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        float interval = .1f;
        float currentTime = interval;
        var sprite = GetComponent<SpriteRenderer>();
        Color originalColor = new Color(
            sprite.color.r,
            sprite.color.g,
            sprite.color.b,
            sprite.color.a
        );

        float[] alphas = { 0, .5f, 1f };
        TwoWayList list = new TwoWayList(alphas);

        while (currentTime < 5f)
        {
            sprite.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                list.GetNext()
            );

            currentTime += interval;

            yield return new WaitForSeconds(interval);
        }

        sprite.color = originalColor;
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

        if (knockBackDirection != Vector2.zero)
        {
            playerMovement.KnockBack(knockBackDirection);
        }

        float newHealth = stat.ModifyHealth(-damage);

        if (newHealth <= 0)
        {
            ProcessDeath();
        }

        isInvincible = true;

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
        float currentTime = .1f;
        var sprite = GetComponent<SpriteRenderer>();
        Color originalColor = new Color(
            sprite.color.r,
            sprite.color.g,
            sprite.color.b,
            sprite.color.a
        );

        float[] alphas = { 0, .5f, 1f };
        TwoWayList list = new TwoWayList(alphas);

        while (currentTime < invincibleTime)
        {
            sprite.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                list.GetNext()
            );

            currentTime += .1f;

            yield return new WaitForSeconds(.1f);
        }

        sprite.color = originalColor;

        isInvincible = false;
    }

    private void ProcessDeath()
    {
        isDead = true;
        onPlayerDeath?.Invoke();
    }

    public float KillCount => killCount;
    public bool IsDead => isDead;

    // public bool HasSkill(string name)
    // {
    //     return skills.ContainsKey(name);
    // }
}

class TwoWayList
{
    private List<float> values;
    private int index;
    private bool forward;

    public TwoWayList(float[] list)
    {
        values = new List<float>(list);

        index = 0;

        forward = true;
    }

    public float GetNext()
    {
        float value = values[index];

        if (forward)
        {
            if (index == values.Count - 1)
            {
                forward = false;
                index--;
            }
            else
            {
                index++;
            }
        }
        else
        {
            if (index == 0)
            {
                forward = true;
                index++;
            }
            else
            {
                index--;
            }
        }

        return value;
    }
}
