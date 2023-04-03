using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerStatus : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private SoundManager soundManager;
    private GameSession gameSession;

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
    private const int MAX_ATTACK_SKILL_COUNT = 4;
    private const int MAX_UTILITY_SKILL_COUNT = 3;

    /////////////////////////////////////////////////////

    ////////////////////// STATUS //////////////////////
    private bool isDead,
        isInvincible;
    private float invincibleTime = .75f;

    /////////////////////////////////////////////////////

    ////////////////////// LEVEL UP //////////////////////
    public event Action<LevelUpUISkillCardData[]> onLevelUp;
    private bool readyForLevelUp;

    /////////////////////////////////////////////////////


    private void Awake()
    {
        GameManager.RegisterPlayerStatus(this);

        _stat = new PlayerStat(baseHealth, baseDamage, baseSpeed * DEFAULT_MOVE_SPEED_MULTIPLYER);

        isDead = isInvincible = readyForLevelUp = false;

        playerMovement = GetComponent<PlayerMovement>();

        skills = new Dictionary<string, Skill>();
    }

    private void Start()
    {
        soundManager = GameManager.SoundManager();
        gameSession = GameManager.GameSession();
    }

    private void Update()
    {
        if (isDead || gameSession.GamePaused)
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

        if (skillToAdd is SkillConsumable)
        {
            skillToAdd.Use();
            return;
        }

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

        var sprite = playerMovement.SpriteRender;
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

    public void Heal(float amount)
    {
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

    //TODO: this is for testing level up ui, THIS IS TEMPORARY ! SHOULD DELETE
    public LevelUpUISkillCardData[] AvailableLevelUpSkills()
    {
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

        var availableSkills = GenerateLevelUpSkills();

        List<Skill> levelUpSkills = new List<Skill>();

        Func<List<Skill>, List<Skill>> getThreeRandomSkills = availSkills =>
        {
            if (availSkills.Count <= 3)
            {
                return availSkills;
            }

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

        levelUpSkills = getThreeRandomSkills(availableSkills);

        while (levelUpSkills.Count < 3)
        {
            levelUpSkills.Add(SkillConsumable.GenerateRandomConsumable());
        }

        List<LevelUpUISkillCardData> skillCardDatum = new List<LevelUpUISkillCardData>();

        foreach (var skill in levelUpSkills)
        {
            string displayName;
            Sprite sprite;
            string description;
            int level;

            if (skill is SkillConsumable)
            {
                var skillCon = (SkillConsumable)skill;
                displayName = skillCon.displayName;
                sprite = skillCon.sprite;
                description = skillCon.description;
                level = 0;
            }
            else
            {
                string skillName = skill.name.ToLower();
                displayName = GameManager.GetSkillData(skillName).displayName;
                sprite = GameManager.GetSkillData(skillName).sprite;
                description = GameManager.GetSkillData(skillName).description;
                level = 1;

                if (skills.TryGetValue(skillName, out Skill s))
                {
                    displayName += " Lv" + (s.Level() + 1).ToString();
                    level = s.Level() + 1;

                    switch (s.Level() + 1)
                    {
                        case 2:
                            description = GameManager.GetSkillData(skillName).lv2Effect;
                            break;
                        case 3:
                            description = GameManager.GetSkillData(skillName).lv3Effect;
                            break;
                        case 4:
                            description = GameManager.GetSkillData(skillName).lv4Effect;
                            break;
                        case 5:
                            description = GameManager.GetSkillData(skillName).lv5Effect;
                            break;
                        default:
                            break;
                    }
                }
            }

            skillCardDatum.Add(
                new LevelUpUISkillCardData(
                    displayName,
                    sprite,
                    description,
                    level,
                    () =>
                    {
                        AssignSkill(skill);
                        readyForLevelUp = true;
                        FindObjectOfType<LevelUpUITest>().Hide();
                    }
                )
            );
        }

        // foreach (var card in skillCardDatum)
        // {
        //     Debug.Log(
        //         card.nameText
        //             + " | "
        //             + card.iconSprite
        //             + " | "
        //             + card.descriptionText
        //             + " | "
        //             + card.onSelect
        //     );
        // }

        return skillCardDatum.ToArray<LevelUpUISkillCardData>();
    }

    private IEnumerator HandleLevelUp(int levelUps)
    {
        // Show level up UI
        onLevelUp?.Invoke(AvailableLevelUpSkills());
        gameSession.PauseGame();
        readyForLevelUp = false;

        // Wait for level up UI to close
        while (!readyForLevelUp)
        {
            yield return null;
        }

        levelUps--;

        if (levelUps > 0)
        {
            StartCoroutine(HandleLevelUp(levelUps));
        }
        else
        {
            gameSession.ResumeGame();
        }
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
