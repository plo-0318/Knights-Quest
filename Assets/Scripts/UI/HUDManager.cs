using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum HUD_ELEMENTS {
    HpBar,
    HpBarBuffer,
    ExpBar
}

public class HUDManager : MonoBehaviour 
{
    [Header("Player Status Bar")] 
    [SerializeField] private Image currentHpBar;
    [SerializeField] private Image bufferHpBar;
    [SerializeField] private TextMeshProUGUI hpBarLabel;
    [SerializeField] private int currentHp;
    [SerializeField] private int maxHp;
    [SerializeField] private float damageTimeBuffer = 1f;
    [SerializeField] private float healTimeBuffer = 1f;
    private float currentDamageTimeBuffer;
    private bool startDamageBuffer;
    private float currentHealTimeBuffer;
    private bool startHealBuffer;
    
    [Header("EXP Bar")]
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI expLevelLabel;
    [SerializeField] private int currentLevel;
    [SerializeField] private int currentExp;
    [SerializeField] private int nextMaxExp;
    [SerializeField] private int maxExp;

    [Header("Kill Counter")] 
    [SerializeField] private TextMeshProUGUI killCountLabel;
    [SerializeField] private int killCount;

    [Header("Stopwatch")] 
    [SerializeField] private TextMeshProUGUI stopwatchLabel;
    [SerializeField] private float currentTime;
    [SerializeField] private bool isStopwatchActive;

    [Header("Player Skills")]
    [SerializeField] private GameObject skill1;
    [SerializeField] private SkillScriptableObject skillData1;
    [SerializeField] private int currentLevelSkill1 = 1;
    
    [SerializeField] private GameObject skill2;
    [SerializeField] private SkillScriptableObject skillData2;
    [SerializeField] private int currentLevelSkill2 = 1;

    [SerializeField] private GameObject skill3;
    [SerializeField] private SkillScriptableObject skillData3;
    [SerializeField] private int currentLevelSkill3 = 1;

    [SerializeField] private GameObject skill4;
    [SerializeField] private SkillScriptableObject skillData4;
    [SerializeField] private int currentLevelSkill4 = 1;

    // Start is called before the first frame update
    void Start() 
    {
        // Enable HUD UI
        ShowHUD(startOffset: 1f);

        // TODO DEFINE INITIAL VALUES
        
        // Player HP
        UpdateHpLabel();
        currentHpBar.fillAmount = 1f;
        bufferHpBar.fillAmount = 1f;
        
        // Player Exp
        expBar.fillAmount = 0f;
        UpdateExpLabel();

        // Kill Count
        killCountLabel.text = $"{killCount}";
        
        // Stopwatch
        currentTime = 0;
        
        // Player Skills
        UpdateSkillsUI();
    }

    // Update is called once per frame
    void Update()
    {
        // Damage Buffer Animation
        if (startDamageBuffer) 
        {
            if (currentDamageTimeBuffer <= 0f) 
            {
                // Update HP Bar Buffer
                UpdateBarValue(bufferHpBar.gameObject, bufferHpBar.fillAmount, (float) currentHp/maxHp, HUD_ELEMENTS.HpBarBuffer);
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
                UpdateBarValue(currentHpBar.gameObject, currentHpBar.fillAmount, (float) currentHp/maxHp, HUD_ELEMENTS.HpBar);
                currentHealTimeBuffer = healTimeBuffer;
                startHealBuffer = false;
            }
            
            currentHealTimeBuffer -= Time.deltaTime;
        }
        
        // -- Stopwatch --
        if (isStopwatchActive) {
            currentTime += Time.deltaTime;
        }
        
        var time = TimeSpan.FromSeconds(currentTime);
        var minutesText = (time.Minutes < 10) ? $"0{time.Minutes}" : $"{time.Minutes}";
        var secondsText = (time.Seconds < 10) ? $"0{time.Seconds}" : $"{time.Seconds}";
        stopwatchLabel.text = minutesText + ":" + secondsText;
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
        else {
            currentHp -= value;
        }

        // Update HP Bar
        UpdateBarValue(currentHpBar.gameObject, currentHpBar.fillAmount, (float) currentHp/maxHp, HUD_ELEMENTS.HpBar);
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
        UpdateBarValue(bufferHpBar.gameObject, bufferHpBar.fillAmount, (float) currentHp/maxHp, HUD_ELEMENTS.HpBarBuffer);
        UpdateHpLabel();
    }

    private void UpdateHpLabel() {
        hpBarLabel.text = $"{currentHp}/{maxHp} HP";
    }

    // --- PLAYER EXP BAR ---

    public void UpdatePlayerExp(int value) 
    {
        if (value >= maxExp || value + currentExp >= maxExp) 
        {
            LevelUp();
        }
        else 
        {
            currentExp += value;
        }
        
        // Update Exp Bar
        UpdateBarValue(expBar.gameObject, expBar.fillAmount, (float) currentExp/maxExp, HUD_ELEMENTS.ExpBar);
        UpdateExpLabel();
    }

    private void LevelUp() 
    {
        // TODO: TRIGGER UI LEVEL UP SCREEN
        
        // Update Values
        currentLevel++;
        maxExp = nextMaxExp;
        currentExp = 0;
        
        // Update Exp Bar
        UpdateBarValue(expBar.gameObject, expBar.fillAmount, 0, HUD_ELEMENTS.ExpBar);
        UpdateExpLabel();
    }

    private void UpdateExpLabel() {
        expLevelLabel.text = $"{currentLevel}";
    }
    
    // -- Kill Counter --

    public void IncreaseKillCounter() {
        killCount++;
        killCountLabel.text = $"{killCount}";
    }
    
    // -- Player Skills --
    public void UpdateSkillsUI() {
        var skillPlaceholder1 = skill1.transform.Find("SkillPlaceholder").gameObject.GetComponent<Image>();
        var skillLevelPlaceholder1 = skill1.transform.Find("LevelPlaceholder").gameObject.GetComponent<Image>();
        var skillIcon1 = skill1.transform.Find("SkillIcon").gameObject.GetComponent<Image>();
        var skillLevelLabel1 = skill1.transform.Find("LevelLabel").GetComponent<TextMeshProUGUI>();
        skillPlaceholder1.color = new Color(255, 255, 255, (skillData1 != null) ? 1f : 0.5f);
        skillLevelPlaceholder1.color = new Color(255, 255, 255, (skillData1 != null) ? 1f : 0f);
        skillIcon1.sprite = (skillData1 != null) ? skillData1.GetSkillIcon() : null;
        skillIcon1.color = new Color(255, 255, 255, (skillData1 != null) ? 1f : 0f);
        skillLevelLabel1.color = new Color(255, 255, 255, (skillData1 != null) ? 1f : 0f);
        skillLevelLabel1.text = currentLevelSkill1.ToString();

        var skillPlaceholder2 = skill2.transform.Find("SkillPlaceholder").gameObject.GetComponent<Image>();
        var skillLevelPlaceholder2 = skill2.transform.Find("LevelPlaceholder").gameObject.GetComponent<Image>();
        var skillIcon2 = skill2.transform.Find("SkillIcon").gameObject.GetComponent<Image>();
        var skillLevelLabel2 = skill2.transform.Find("LevelLabel").GetComponent<TextMeshProUGUI>();
        skillPlaceholder2.color = new Color(255, 255, 255, (skillData2 != null) ? 1f : 0.5f);
        skillLevelPlaceholder2.color = new Color(255, 255, 255, (skillData2 != null) ? 1f : 0f);
        skillIcon2.sprite = (skillData2 != null) ? skillData2.GetSkillIcon() : null;
        skillIcon2.color = new Color(255, 255, 255, (skillData2 != null) ? 1f : 0f);
        skillLevelLabel2.color = new Color(255, 255, 255, (skillData2 != null) ? 1f : 0f);
        skillLevelLabel2.text = currentLevelSkill2.ToString();

        var skillPlaceholder3 = skill3.transform.Find("SkillPlaceholder").gameObject.GetComponent<Image>();
        var skillLevelPlaceholder3 = skill3.transform.Find("LevelPlaceholder").gameObject.GetComponent<Image>();
        var skillIcon3 = skill3.transform.Find("SkillIcon").gameObject.GetComponent<Image>();
        var skillLevelLabel3 = skill3.transform.Find("LevelLabel").GetComponent<TextMeshProUGUI>();
        skillPlaceholder3.color = new Color(255, 255, 255, (skillData3 != null) ? 1f : 0.5f);
        skillLevelPlaceholder3.color = new Color(255, 255, 255, (skillData3 != null) ? 1f : 0f);
        skillIcon3.sprite = (skillData3 != null) ? skillData3.GetSkillIcon() : null;
        skillIcon3.color = new Color(255, 255, 255, (skillData3 != null) ? 1f : 0f);
        skillLevelLabel3.color = new Color(255, 255, 255, (skillData3 != null) ? 1f : 0f);
        skillLevelLabel3.text = currentLevelSkill3.ToString();

        var skillPlaceholder4 = skill4.transform.Find("SkillPlaceholder").gameObject.GetComponent<Image>();
        var skillLevelPlaceholder4 = skill4.transform.Find("LevelPlaceholder").gameObject.GetComponent<Image>();
        var skillIcon4 = skill4.transform.Find("SkillIcon").gameObject.GetComponent<Image>();
        var skillLevelLabel4 = skill4.transform.Find("LevelLabel").GetComponent<TextMeshProUGUI>();
        skillPlaceholder4.color = new Color(255, 255, 255, (skillData4 != null) ? 1f : 0.5f);
        skillLevelPlaceholder4.color = new Color(255, 255, 255, (skillData4 != null) ? 1f : 0f);
        skillIcon4.sprite = (skillData4 != null) ? skillData4.GetSkillIcon() : null;
        skillIcon4.color = new Color(255, 255, 255, (skillData4 != null) ? 1f : 0f);
        skillLevelLabel4.color = new Color(255, 255, 255, (skillData4 != null) ? 1f : 0f);
        skillLevelLabel4.text = currentLevelSkill4.ToString();
    }

    public void LevelUpSkill(int playerSkill) {
        switch (playerSkill) {
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
        
        LeanTween.value(obj, current, target, 0.5f).setOnUpdate((float val) => {
            switch (type) {
                case HUD_ELEMENTS.HpBar:
                    currentHpBar.fillAmount = val;
                    break;
                case HUD_ELEMENTS.HpBarBuffer:
                    bufferHpBar.fillAmount = val;
                    break;
                case HUD_ELEMENTS.ExpBar:
                    expBar.fillAmount = val;
                    break;
            }
        }).setEaseOutExpo();
    }
}
