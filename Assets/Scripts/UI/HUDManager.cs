using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

enum HUD_ELEMENTS
{
    HpBar,
    HpBarBuffer,
    BossHpBar,
    BossHpBarBuffer,
    ExpBar
}

public class HUDManager : MonoBehaviour
{
    [Header("Player Status Bar")]
    [SerializeField]
    private Image currentHpBar;

    [SerializeField]
    private Image bufferHpBar;

    [SerializeField]
    private TextMeshProUGUI hpBarLabel;

    [SerializeField]
    private int currentHp;

    [SerializeField]
    private int maxHp;

    [SerializeField]
    private float damageTimeBuffer = 1f;

    [SerializeField]
    private float healTimeBuffer = 1f;
    private float currentDamageTimeBuffer;
    private bool startDamageBuffer;
    private float currentHealTimeBuffer;
    private bool startHealBuffer;

    [Header("EXP Bar")]
    [SerializeField]
    private Image expBar;

    [SerializeField]
    private TextMeshProUGUI expLevelLabel;

    [SerializeField]
    private int currentLevel;

    [SerializeField]
    private int currentExp;

    [SerializeField]
    private int maxExp;

    [SerializeField]
    private UnityEvent onLevelUpAction;

    [Header("Kill Counter")]
    [SerializeField]
    private TextMeshProUGUI killCountLabel;

    [SerializeField]
    private int killCount;

    [Header("Stopwatch")]
    [SerializeField]
    private TextMeshProUGUI stopwatchLabel;

    [Header("Player Skills")]
    [SerializeField]
    private GameObject skill1;

    [SerializeField]
    private SkillScriptableObject skillData1;

    [SerializeField]
    private int currentLevelSkill1 = 1;

    [SerializeField]
    private GameObject skill2;

    [SerializeField]
    private SkillScriptableObject skillData2;

    [SerializeField]
    private int currentLevelSkill2 = 1;

    [SerializeField]
    private GameObject skill3;

    [SerializeField]
    private SkillScriptableObject skillData3;

    [SerializeField]
    private int currentLevelSkill3 = 1;

    [SerializeField]
    private GameObject skill4;

    [SerializeField]
    private SkillScriptableObject skillData4;

    [SerializeField]
    private int currentLevelSkill4 = 1;
    private int currentNumberOfSkills = 0;

    [Header("Boss Hp")]
    [SerializeField]
    private GameObject bossHpBar;

    [SerializeField]
    private Image currentBossHpBar;

    [SerializeField]
    private Image currentBossBufferHpBar;

    [SerializeField]
    private TextMeshProUGUI bossHpLabel;

    [SerializeField]
    private int currentBossHp;

    [SerializeField]
    private int currentBossMaxHp;

    [SerializeField]
    private int currentBossBufferHp;

    [SerializeField]
    private float bossDamageTimeBuffer;

    [SerializeField]
    private float bossHealingTimeBuffer;
    private float currentBossDamageTimeBuffer;
    private bool startBossDamageBuffer;
    private float currentBossHealTimeBuffer;
    private bool startBossHealBuffer;
    public Enemy currentBoss;

    private bool bossUIOnScreen = false;

    // Player Status
    private PlayerStatus playerStatus;
    private GameSession gameSession;
    private SoundManager soundManager;

    private void Awake()
    {
        GameManager.RegisterHUDManager(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setup Game Managers
        playerStatus = GameManager.PlayerStatus();
        gameSession = GameManager.GameSession();
        soundManager = GameManager.SoundManager();

        // Enable HUD UI
        // ShowHUD(startOffset: 1f);

        // Player HP
        currentHp = (int)playerStatus.Health;
        maxHp = (int)playerStatus.GetStat(Stat.MAX_HEALTH);
        UpdateHpLabel();
        currentHpBar.fillAmount = 1f;
        bufferHpBar.fillAmount = 1f;

        // Player Exp
        currentExp = (int)playerStatus.Exp;
        currentLevel = playerStatus.Level;
        maxExp = (int)playerStatus.ExpForNextLevel(currentLevel + 1);
        expBar.fillAmount = 0f;
        UpdateExpLabel();

        // Kill Count
        killCountLabel.text = $"{playerStatus.KillCount}";

        // Player Skills
        UpdateSkillsUI();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if HP Changed
        if (playerStatus.Health < currentHp)
        {
            DecreaseHealth(Mathf.Abs(currentHp - (int)playerStatus.Health));
        }
        else if (playerStatus.Health > currentHp)
        {
            IncreaseHealth(Mathf.Abs(currentHp - (int)playerStatus.Health));
        }

        // Check if EXP Changed
        // if (playerStatus.Exp > currentExp)
        // {
        //     UpdatePlayerExp(Mathf.Abs(currentExp - (int)playerStatus.Exp));
        // }

        // If UI exp is not the same as the actual player exp, update the ui exp
        if (Mathf.Abs(playerStatus.Exp - currentExp) > Mathf.Epsilon)
        {
            UpdatePlayerExp((int)playerStatus.Exp);
        }

        // Check if Skill Count Changed
        UpdateSkillsUI();

        // Check if the boss is spawned
        currentBoss = EnemySpawnUtil.CurrentBoss;

        if (currentBoss != null)
        {
            if (bossUIOnScreen == false)
            {
                ShowBossUI();
            }

            if (currentBoss.Health < currentBossHp)
            {
                DecreaseBossHealth(Mathf.Abs(currentBossHp - (int)currentBoss.Health));
            }

            if (currentBossHp <= 0)
            {
                HideBossUI();
            }
        }

        // Damage Buffer Animation
        if (startDamageBuffer)
        {
            if (currentDamageTimeBuffer <= 0f)
            {
                // Update HP Bar Buffer
                UpdateBarValue(
                    bufferHpBar.gameObject,
                    bufferHpBar.fillAmount,
                    (float)currentHp / maxHp,
                    HUD_ELEMENTS.HpBarBuffer
                );
                currentDamageTimeBuffer = damageTimeBuffer;
                startDamageBuffer = false;
            }

            currentDamageTimeBuffer -= Time.deltaTime;
        }

        // Heal Buffer Animation
        if (startHealBuffer)
        {
            if (currentHealTimeBuffer <= 0f)
            {
                // Update HP Bar
                UpdateBarValue(
                    currentHpBar.gameObject,
                    currentHpBar.fillAmount,
                    (float)currentHp / maxHp,
                    HUD_ELEMENTS.HpBar
                );
                currentHealTimeBuffer = healTimeBuffer;
                startHealBuffer = false;
            }

            currentHealTimeBuffer -= Time.deltaTime;
        }

        // Boss Damage Buffer Animation
        if (startBossDamageBuffer)
        {
            if (currentBossDamageTimeBuffer <= 0)
            {
                UpdateBarValue(
                    currentBossBufferHpBar.gameObject,
                    currentBossBufferHpBar.fillAmount,
                    (float)currentBossHp / currentBossMaxHp,
                    HUD_ELEMENTS.BossHpBarBuffer
                );
                currentBossDamageTimeBuffer = bossDamageTimeBuffer;
                startBossDamageBuffer = false;
            }

            currentBossDamageTimeBuffer -= Time.deltaTime;
        }

        // -- Timer --

        var time = TimeSpan.FromSeconds(GameManager.GameSession().CurrentTime);
        var minutesText = (time.Minutes < 10) ? $"0{time.Minutes}" : $"{time.Minutes}";
        var secondsText = (time.Seconds < 10) ? $"0{time.Seconds}" : $"{time.Seconds}";
        stopwatchLabel.text = minutesText + ":" + secondsText;

        // -- Kill Count --
        killCountLabel.text = $"{playerStatus.KillCount}";
    }

    // --- PLAYER HP BAR ---

    public void DecreaseHealth(int value)
    {
        startDamageBuffer = true;
        currentDamageTimeBuffer = damageTimeBuffer;

        if (currentHp <= value)
        {
            currentHp = 0;
        }
        else
        {
            currentHp -= value;
        }

        // Update HP Bar
        UpdateBarValue(
            currentHpBar.gameObject,
            currentHpBar.fillAmount,
            (float)currentHp / maxHp,
            HUD_ELEMENTS.HpBar
        );
        UpdateHpLabel();
    }

    public void IncreaseHealth(int value)
    {
        startHealBuffer = true;
        currentHealTimeBuffer = healTimeBuffer;

        if (value >= maxHp || value + currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        else
        {
            currentHp += value;
        }

        // Update HP Bar Buffer
        UpdateBarValue(
            bufferHpBar.gameObject,
            bufferHpBar.fillAmount,
            (float)currentHp / maxHp,
            HUD_ELEMENTS.HpBarBuffer
        );
        UpdateHpLabel();
    }

    private void UpdateHpLabel()
    {
        hpBarLabel.text = $"{currentHp}/{maxHp} HP";
    }

    // --- PLAYER EXP BAR ---

    // public void UpdatePlayerExp(int value)
    // {
    //     if (value >= maxExp || value + currentExp >= maxExp)
    //     {
    //         LevelUp();
    //     }
    //     else
    //     {
    //         currentExp += value;
    //     }

    //     // Update Exp Bar
    //     UpdateBarValue(
    //         expBar.gameObject,
    //         expBar.fillAmount,
    //         (float)currentExp / maxExp,
    //         HUD_ELEMENTS.ExpBar
    //     );
    //     UpdateExpLabel();
    // }

    public void UpdatePlayerExp(int newExp)
    {
        currentExp = newExp;

        // Update Exp Bar
        UpdateBarValue(
            expBar.gameObject,
            expBar.fillAmount,
            (float)currentExp / playerStatus.ExpToNextLevel,
            HUD_ELEMENTS.ExpBar
        );
        UpdateExpLabel();
    }

    public void ShowLevelUpUI()
    {
        soundManager.PlayClip(soundManager.audioRefs.sfxLevelUpUIOpen);

        onLevelUpAction.Invoke();
        UpdateExpLabel();
    }

    // private void LevelUp()
    // {
    //     // TODO: TRIGGER UI LEVEL UP SCREEN
    //     onLevelUpAction.Invoke();

    //     // Update Values
    //     currentExp = (int)playerStatus.Exp;
    //     currentLevel = playerStatus.Level;
    //     maxExp = (int)playerStatus.ExpForNextLevel(currentLevel + 1);

    //     // Update Exp Bar
    //     UpdateBarValue(expBar.gameObject, expBar.fillAmount, 0, HUD_ELEMENTS.ExpBar);
    //     UpdateExpLabel();
    // }

    private void UpdateExpLabel()
    {
        // expLevelLabel.text = $"{currentLevel}";
        expLevelLabel.text = $"{playerStatus.Level}";
    }

    // -- Kill Counter --

    public void IncreaseKillCounter()
    {
        killCount++;
        killCountLabel.text = $"{killCount}";
    }

    // -- Player Skills --
    public void UpdateSkillsUI()
    {
        var attackSkills = playerStatus.GetSkillDatum().Where(c => c.Type == "ATTACK").ToArray();

        for (var i = 0; i < 4; i++)
        {
            var skillData = (attackSkills.Length > i) ? attackSkills[i] : null;

            var currentSkill = i switch
            {
                0 => skill1,
                1 => skill2,
                2 => skill3,
                3 => skill4,
                _ => skill1
            };

            var skillPlaceholder = currentSkill.transform
                .Find("SkillPlaceholder")
                .gameObject.GetComponent<Image>();
            var skillLevelPlaceholder = currentSkill.transform
                .Find("LevelPlaceholder")
                .gameObject.GetComponent<Image>();
            var skillIcon = currentSkill.transform
                .Find("SkillIcon")
                .gameObject.GetComponent<Image>();
            var skillLevelLabel = currentSkill.transform
                .Find("LevelLabel")
                .GetComponent<TextMeshProUGUI>();
            skillPlaceholder.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0.5f);
            skillLevelPlaceholder.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            skillIcon.sprite = (skillData != null) ? skillData.Sprite : null;
            skillIcon.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            skillLevelLabel.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            if (skillData != null)
                skillLevelLabel.text = skillData.GetCurrentLevel().ToString();
        }
    }

    // TODO REMOVE
    public void LevelUpSkill(int playerSkill)
    {
        switch (playerSkill)
        {
            case 1 when currentLevelSkill1 < 5 && skillData1:
                currentLevelSkill1++;
                break;
            case 2 when currentLevelSkill2 < 5 && skillData2:
                currentLevelSkill2++;
                break;
            case 3 when currentLevelSkill3 < 5 && skillData3:
                currentLevelSkill3++;
                break;
            case 4 when currentLevelSkill4 < 5 && skillData4:
                currentLevelSkill4++;
                break;
        }

        UpdateSkillsUI();
    }

    // -- BOSS --
    private void ShowBossUI()
    {
        bossHpBar.GetComponent<InAndOutAnimation>().MoveInAnimation();

        currentBoss = EnemySpawnUtil.CurrentBoss;
        currentBossHp = (int)currentBoss.Health;
        currentBossMaxHp = (int)currentBoss.GetStat(Stat.MAX_HEALTH);
        currentBossHpBar.fillAmount = 1f;
        currentBossBufferHpBar.fillAmount = 1f;

        bossUIOnScreen = true;
    }

    private void HideBossUI()
    {
        bossHpBar.GetComponent<InAndOutAnimation>().MoveOutAnimation();
        bossUIOnScreen = false;
    }

    public void DecreaseBossHealth(int value)
    {
        startBossDamageBuffer = true;
        currentBossDamageTimeBuffer = bossDamageTimeBuffer;

        if (currentBossHp <= value)
        {
            currentBossHp = 0;
        }
        else
        {
            currentBossHp -= value;
        }

        UpdateBarValue(
            currentBossHpBar.gameObject,
            currentBossHpBar.fillAmount,
            (float)currentBossHp / currentBossMaxHp,
            HUD_ELEMENTS.BossHpBar
        );
    }

    public void IncreaseBossHealth(int value)
    {
        startHealBuffer = true;
        currentHealTimeBuffer = healTimeBuffer;

        if (value >= maxHp || value + currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        else
        {
            currentHp += value;
        }

        // Update HP Bar Buffer
        UpdateBarValue(
            bufferHpBar.gameObject,
            bufferHpBar.fillAmount,
            (float)currentHp / maxHp,
            HUD_ELEMENTS.HpBarBuffer
        );
        UpdateHpLabel();
    }

    // -- GENERAL FUNCTIONS --

    public void ShowHUD(float startOffset = 0f)
    {
        var uiAnimation = gameObject.GetComponent<InAndOutAnimation>();
        uiAnimation.MoveInAnimation(startOffset);
    }

    public void HideHUD(float startOffset = 0f)
    {
        var uiAnimation = gameObject.GetComponent<InAndOutAnimation>();
        uiAnimation.MoveOutAnimation(startOffset);
    }

    // Calculate the length of the bar relative to the max value and the current value
    private void UpdateBarValue(GameObject obj, float current, float target, HUD_ELEMENTS type)
    {
        LeanTween.cancel(obj);

        LeanTween
            .value(obj, current, target, 0.5f)
            .setOnUpdate(
                (float val) =>
                {
                    switch (type)
                    {
                        case HUD_ELEMENTS.HpBar:
                            currentHpBar.fillAmount = val;
                            break;
                        case HUD_ELEMENTS.HpBarBuffer:
                            bufferHpBar.fillAmount = val;
                            break;
                        case HUD_ELEMENTS.ExpBar:
                            expBar.fillAmount = val;
                            break;
                        case HUD_ELEMENTS.BossHpBar:
                            currentBossHpBar.fillAmount = val;
                            break;
                        case HUD_ELEMENTS.BossHpBarBuffer:
                            currentBossBufferHpBar.fillAmount = val;
                            break;
                    }
                }
            )
            .setEaseOutExpo()
            .setIgnoreTimeScale(true);
    }
}
