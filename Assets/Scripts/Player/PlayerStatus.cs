using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public class PlayerStatus : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private SoundManager soundManager;
    private GameSession gameSession;
    private CircleCollider2D itemPickupCollider;

    /////////////////////////////////////////////////////

    ////////////////////// STATS //////////////////////
    [SerializeField]
    private float baseHealth = 1000f,
        baseDamage = 1f,
        baseSpeed = 6f;

    private const float DEFAULT_MOVE_SPEED_MULTIPLYER = 35f;

    private PlayerStat _stat;

    /////////////////////////////////////////////////////

    ////////////////////// SKILLS //////////////////////
    private Dictionary<string, Skill> skills;
    private int numAttackingSkill,
        numUtilitySkill;
    private const int MAX_SKILL_COUNT = 7;
    private const int MAX_ATTACK_SKILL_COUNT = 4;
    private const int MAX_UTILITY_SKILL_COUNT = 3;

    /////////////////////////////////////////////////////

    ////////////////////// STATUS //////////////////////
    private bool isDead,
        isInvincible;

    private float invincibleTime = .75f;

    /////////////////////////////////////////////////////

    ////////////////////// LEVEL UP //////////////////////
    // public event Action<LevelUpUISkillCardData> onLevelUp;
    public event Action<SkillData[]> onLevelUp;
    private bool readyForLevelUp;

    /////////////////////////////////////////////////////


    private void Awake()
    {
        GameManager.RegisterPlayerStatus(this);

        itemPickupCollider = GetComponentInChildren<CollectablePickup>()
            .GetComponent<CircleCollider2D>();

        _stat = new PlayerStat(
            baseHealth,
            baseDamage,
            baseSpeed * DEFAULT_MOVE_SPEED_MULTIPLYER,
            itemPickupCollider.radius
        );

        isDead = isInvincible = false;
        readyForLevelUp = false;

        playerMovement = GetComponent<PlayerMovement>();

        skills = new Dictionary<string, Skill>();
        numAttackingSkill = numUtilitySkill = 0;
    }

    private void Start()
    {
        soundManager = GameManager.SoundManager();
        gameSession = GameManager.GameSession();
    }

    private void Update()
    {
        if (isDead || gameSession.IsGamePaused || gameSession.IsGameOver)
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
        if (!skillToAdd)
        {
            return;
        }

        // If it is a consumable skill, just use it
        if (skillToAdd is SkillConsumable)
        {
            skillToAdd.Use();
            return;
        }

        if (!AllowedToAddSkill(skillToAdd))
        {
            return;
        }

        // If player has not learned this skill, add it
        if (!skills.TryGetValue(skillToAdd.name, out Skill skill))
        {
            if (skillToAdd.SkillType == Skill.Type.ATTACK)
            {
                numAttackingSkill++;
            }
            else if (skillToAdd.SkillType == Skill.Type.UTILITY)
            {
                numUtilitySkill++;
            }

            skills.Add(skillToAdd.name, skillToAdd);
        }
        // else level up this skill
        else
        {
            skill.Upgrade();
        }
    }

    private bool AllowedToAddSkill(Skill skillToAdd)
    {
        // Check if the player has the maximum number of skills

        if (
            numAttackingSkill >= MAX_ATTACK_SKILL_COUNT && skillToAdd.SkillType == Skill.Type.ATTACK
        )
        {
            return skills.ContainsKey(skillToAdd.name);
        }

        if (
            numUtilitySkill >= MAX_UTILITY_SKILL_COUNT && skillToAdd.SkillType == Skill.Type.UTILITY
        )
        {
            return skills.ContainsKey(skillToAdd.name);
        }

        return true;
    }

    public Dictionary<string, Skill> GetSkills()
    {
        return skills;
    }

    public float GetStat(int statType)
    {
        return _stat.GetStat(statType);
    }

    private bool BreakShield()
    {
        Skill skill = null;

        if (!skills.TryGetValue("shield", out skill))
        {
            return false;
        }

        if (!skill || !(skill is SkillShield))
        {
            return false;
        }

        SkillShield skillShield = (SkillShield)skill;

        return skillShield.BreakShield();
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

        // If shield breaks successfully, do not take damage
        if (BreakShield())
        {
            isInvincible = true;
            StartCoroutine(RecoverFromShieldBreak());

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

    private IEnumerator RecoverFromShieldBreak()
    {
        yield return new WaitForSeconds(invincibleTime);

        isInvincible = false;
    }

    private IEnumerator RecoverFromInvincible()
    {
        var sprite = playerMovement.SpriteRender;

        yield return StartCoroutine(AnimationUtil.BlinkSprite(sprite, invincibleTime));

        isInvincible = false;
    }

    public void Heal(float amount)
    {
        DamagePopup.ShowHealPopup(amount, transform, Quaternion.identity, transform);

        soundManager.PlayClip(soundManager.audioRefs.sfxPickupPotion);

        _stat.ModifyHealth(amount);
    }

    public void IncreaseExp(float amount)
    {
        int levelUps = _stat.IncreaseExp(amount);

        if (levelUps > 0)
        {
            StartCoroutine(HandleLevelUp(levelUps));
        }
    }

    // Return a function that, when skill is clicked in the level up ui
    // levels up the selected skill
    public UnityAction OnUISkillSelect(Skill skill)
    {
        return () =>
        {
            AssignSkill(skill);
            readyForLevelUp = true;
            GameManager.PauseManager().EnablePause();
        };
    }

    private List<Skill> GenerateLevelUpSkills()
    {
        //TODO: delete this test
        // sword
        // dagger
        // field
        // arrow
        // fireball
        // hammer
        // bolt
        // var skills = new Dictionary<string, Skill>();
        // var skillSword = new SkillSword();
        // var skillDagger = new SkillDagger();
        // var skillFIeld = new SkillField();
        // var skillArrow = new SkillArrow();
        // var skillFireball = new SkillFireball();
        // var skillHammer = new SkillHammer();

        // var skillBoots = new SkillBoots();
        // var skillShield = new SkillShield();
        // var skillCrystal = new SkillCrystal();
        // var skillGauntlet = new SkillGauntlet();

        // skills.Add(skillArrow.name, skillArrow);
        // skillArrow.Upgrade();
        // skillArrow.Upgrade();
        // skillArrow.Upgrade();
        // skills.Add(skillDagger.name, skillDagger);
        // skillDagger.Upgrade();
        // skillDagger.Upgrade();
        // skillDagger.Upgrade();
        // skills.Add(skillShield.name, skillShield);
        // skills.Add(skillGauntlet.name, skillGauntlet);

        // Get all the skills
        var allSkillData = GameManager.GetAllSkillData();

        // Create a copy of the skills
        IDictionary<string, SkillData> availableSkills = new Dictionary<string, SkillData>(
            allSkillData
        );

        // Stores the name of the skills that is going to be filtered out
        ISet<string> unavailableSkills = new HashSet<string>();

        // Keep track of the current attacking skills and utility skills
        ISet<string> currentAttackingSkills = new HashSet<string>();
        ISet<string> currentUtilitySkills = new HashSet<string>();
        // Looping through the current skills to check for filter options
        foreach (var kvp in skills)
        {
            // If the player has any lv5 skills, add them to the filterOut
            if (kvp.Value.Level() >= 5)
                unavailableSkills.Add(kvp.Key.ToLower());

            // Sum up the count of skill types
            if (kvp.Value.SkillType == Skill.Type.ATTACK)
                currentAttackingSkills.Add(kvp.Key.ToLower());
            if (kvp.Value.SkillType == Skill.Type.UTILITY)
                currentUtilitySkills.Add(kvp.Key.ToLower());
        }

        // If the player has maximum number of attacking skills or utility skills,
        // only keep the skills of that type
        if (currentAttackingSkills.Count >= MAX_ATTACK_SKILL_COUNT)
        {
            ISet<string> availableAttackingSkills = Util.MapDictionaryKeyToSet<string, SkillData>(
                GameManager.GetAllAttackingSkillData()
            );

            // Difference --> Get the unavailable skills
            availableAttackingSkills.ExceptWith(currentAttackingSkills);

            // Union --> Add the unavailable skills to the list
            unavailableSkills.UnionWith(availableAttackingSkills);
        }

        if (currentUtilitySkills.Count >= MAX_UTILITY_SKILL_COUNT)
        {
            ISet<string> availableUtilitySkills = Util.MapDictionaryKeyToSet<string, SkillData>(
                GameManager.GetAllUtilitySkillData()
            );

            // Difference --> Get the unavailable skills
            availableUtilitySkills.ExceptWith(currentUtilitySkills);

            // Union --> Add the unavailable skills to the list
            unavailableSkills.UnionWith(availableUtilitySkills);
        }

        // Filter out all the unavailable skills
        availableSkills = Util.FilterDictionary<string, SkillData>(
            availableSkills,
            unavailableSkills,
            false
        );

        return SkillData.GetSkills(availableSkills.Keys);
    }

    private IEnumerator HandleLevelUp(int levelUps)
    {
        // Disable pause control
        GameManager.PauseManager().DisablePause();

        // Show level up UI
        onLevelUp?.Invoke(AvailableLevelUpSkillData());
        gameSession.PauseGame();
        readyForLevelUp = false;

        GameManager.HUDManager().ShowLevelUpUI();

        // Wait for level up UI to close
        while (!readyForLevelUp)
        {
            yield return null;
        }

        levelUps--;

        if (levelUps > 0 && !isDead)
        {
            gameSession.ResumeGame();
            yield return new WaitForSeconds(0.5f);

            StartCoroutine(HandleLevelUp(levelUps));
        }
        else
        {
            gameSession.ResumeGame();
        }
    }

    public void IncrementKillCount()
    {
        _stat.IncrementKillCount();
    }

    public void AddModifier(Modifier modifier, bool replaceIfExits = true)
    {
        if (modifier != null && modifier.statType == PlayerStat.ITEM_PICKUP_RADIUS)
        {
            itemPickupCollider.radius = _stat.AddModifier(modifier, true);
            return;
        }

        _stat.AddModifier(modifier, replaceIfExits);
    }

    public void RemoveModifier(Modifier modifier)
    {
        _stat.RemoveModifier(modifier);
    }

    private void ProcessDeath()
    {
        isDead = true;

        GameManager.GameSession().HandleGameLost();
    }

    public SkillData[] GetSkillDatum()
    {
        List<SkillData> skillDatum = new List<SkillData>();

        foreach (string skillName in skills.Keys)
        {
            skillDatum.Add(GameManager.GetSkillData(skillName));
        }

        return skillDatum.ToArray();
    }

    public int Level => _stat.level;
    public float Exp => _stat.exp;
    public int KillCount => _stat.killCount;
    public float Health => _stat.health;
    public bool IsDead => isDead;

    public float ExpForNextLevel(int level)
    {
        return _stat.ExpNeededToLevelUp(level);
    }

    public float ExpToNextLevel => _stat.ExpNeededToLevelUp(_stat.level + 1);

    public SkillData[] AvailableLevelUpSkillData()
    {
        // Get the all the available level up skills
        List<Skill> availableSkills = GenerateLevelUpSkills();

        // Create a list for storing the skills that will be displayed
        List<Skill> levelUpSkills = new List<Skill>();

        Func<List<Skill>, List<Skill>> GetThreeRandomSkills = availSkills =>
        {
            // If there are 3 or less available level up skills, just use them
            if (availSkills.Count <= 3)
            {
                return availSkills;
            }

            // Else, pick 3 random skills from the available level up skills
            List<Skill> tempSkills = new List<Skill>();

            while (tempSkills.Count < 3)
            {
                int rand = UnityEngine.Random.Range(0, availSkills.Count);

                if (!tempSkills.Contains(availSkills[rand]))
                {
                    tempSkills.Add(availSkills[rand]);
                }
            }

            return tempSkills;
        };

        // Generate 3 random level up skills
        levelUpSkills = GetThreeRandomSkills(availableSkills);

        // While there are less than 3 level up skills
        while (levelUpSkills.Count < 3)
        {
            // Add random consumable skill
            levelUpSkills.Add(SkillConsumable.GenerateRandomConsumable());
        }

        // Create a list of skill data
        List<SkillData> skillDatum = new List<SkillData>();

        // Convert the level up skill to skill data format
        foreach (var skill in levelUpSkills)
        {
            // If the skill is a regular skill
            if (!(skill is SkillConsumable))
            {
                skillDatum.Add(GameManager.GetSkillData(skill.name.ToLower()));
            }
            // If the skill is a consumable skill
            else
            {
                skillDatum.Add(SkillData.FromSkillConsumable((SkillConsumable)skill));
            }
        }

        // Return the skill data list
        return skillDatum.ToArray();
    }
}
