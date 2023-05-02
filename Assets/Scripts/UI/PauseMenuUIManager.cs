using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuUIManager : MonoBehaviour 
{
    [Header("Pause Screens")]    
    [SerializeField] private GameObject pauseMenuScreen;
    [SerializeField] private GameObject optionsScreen;
    
    [Header("Pause Menu Buttons")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UnityEvent onClickContinue;
    [SerializeField] private GameObject optionsButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private UnityEvent onClickQuit;

    [Header("Options Menu Buttons")] 
    [SerializeField] private GameObject optionsBackButton;
    
    [Header("Pause Menu Skills")]
    [SerializeField] private GameObject[] combatSkills;
    [SerializeField] private GameObject[] utilitySkills;

    private PlayerStatus playerStatus;

    // Start is called before the first frame update
    void Start() {
        // Setup Player Status
        playerStatus = GameManager.PlayerStatus();
        
        // Pause Menu - Listeners
        var continueBtn = continueButton.GetComponent<Button>();
        var continueBtnEvents = continueButton.GetComponent<ButtonEventHandler>();
        continueBtn.onClick.AddListener(delegate { onClickContinue.Invoke(); });
        continueBtnEvents.onSelectAction.AddListener(delegate { OnSelectButton(continueButton); });
        continueBtnEvents.onDeselectAction.AddListener(delegate { OnDeselectButton(continueButton); });
        
        var optionsBtn = optionsButton.GetComponent<Button>();
        var optionsBtnEvents = optionsButton.GetComponent<ButtonEventHandler>();
        optionsBtn.onClick.AddListener(NavigateToOptions);
        optionsBtnEvents.onSelectAction.AddListener(delegate { OnSelectButton(optionsButton); });
        optionsBtnEvents.onDeselectAction.AddListener(delegate { OnDeselectButton(optionsButton); }); 
        
        var quitBtn = quitButton.GetComponent<Button>();
        var quitBtnEvents = quitButton.GetComponent<ButtonEventHandler>();
        quitBtn.onClick.AddListener(delegate { onClickQuit.Invoke(); });
        quitBtnEvents.onSelectAction.AddListener(delegate { OnSelectButton(quitButton); });
        quitBtnEvents.onDeselectAction.AddListener(delegate { OnDeselectButton(quitButton); });
        
        // Options Menu - Listeners
        var optionsBackBtn = optionsBackButton.GetComponent<Button>();
        var optionsBackBtnEvents = optionsBackButton.GetComponent<ButtonEventHandler>();
        optionsBackBtn.onClick.AddListener(NavigateToPause);
        optionsBackBtnEvents.onSelectAction.AddListener(delegate { OnSelectButton(optionsBackButton); });
        optionsBackBtnEvents.onDeselectAction.AddListener(delegate { OnDeselectButton(optionsBackButton); });
    }
    private void NavigateToPause() 
    {
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenuScreen.SetActive(true);
        optionsScreen.SetActive(false);
    }
    
    private void NavigateToOptions() 
    {
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenuScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

    private void UpdateSkillUI() 
    {
        var combatSkillData = playerStatus.GetSkillDatum().Where(c => c.Type == "ATTACK").ToArray();
        
        for (var i = 0; i < combatSkills.Length; i++) {
            var skillData = (combatSkillData.Length > i) ? combatSkillData[i] : null;
            var currentSkill = combatSkills[i];

            var skillTitle = currentSkill.transform.Find("SkillTitle").gameObject.GetComponent<TextMeshProUGUI>();
            var skillDescription = currentSkill.transform.Find("SkillDescription").gameObject.GetComponent<TextMeshProUGUI>();
            skillTitle.text = (skillData != null) ? skillData.DisplayName : "";
            skillDescription.text = (skillData != null) ? skillData.Description : "";
            
            var skillLevelPlaceholder = currentSkill.transform.Find("Skill/LevelPlaceholder").gameObject.GetComponent<Image>();
            var skillIcon = currentSkill.transform.Find("Skill/SkillIcon").gameObject.GetComponent<Image>();
            var skillLevelLabel = currentSkill.transform.Find("Skill/LevelLabel").GetComponent<TextMeshProUGUI>();
            skillLevelPlaceholder.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            skillIcon.sprite = (skillData != null) ? skillData.Sprite : null;
            skillIcon.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            skillLevelLabel.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            if (skillData != null) skillLevelLabel.text = skillData.GetCurrentLevel().ToString();
        }
        
        var utilitySkillData = playerStatus.GetSkillDatum().Where(c => c.Type == "UTILITY").ToArray();

        for (var i = 0; i < utilitySkills.Length; i++) {
            var skillData = (utilitySkillData.Length > i) ? utilitySkillData[i] : null;
            var currentSkill = utilitySkills[i];

            var skillTitle = currentSkill.transform.Find("SkillTitle").gameObject.GetComponent<TextMeshProUGUI>();
            var skillDescription = currentSkill.transform.Find("SkillDescription").gameObject.GetComponent<TextMeshProUGUI>();
            skillTitle.text = (skillData != null) ? skillData.DisplayName : "";
            skillDescription.text = (skillData != null) ? skillData.Description : "";
            
            var skillLevelPlaceholder = currentSkill.transform.Find("Skill/LevelPlaceholder").gameObject.GetComponent<Image>();
            var skillIcon = currentSkill.transform.Find("Skill/SkillIcon").gameObject.GetComponent<Image>();
            var skillLevelLabel = currentSkill.transform.Find("Skill/LevelLabel").GetComponent<TextMeshProUGUI>();
            skillLevelPlaceholder.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            skillIcon.sprite = (skillData != null) ? skillData.Sprite : null;
            skillIcon.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            skillLevelLabel.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            if (skillData != null) skillLevelLabel.text = skillData.GetCurrentLevel().ToString();
        }
    }

    // Public Functions
    public void ShowPauseMenu() {

        Time.timeScale = 0;
        
        UpdateSkillUI();
        
        var menuAnimation = gameObject.GetComponent<InAndOutAnimation>();
        menuAnimation.MoveInAnimation();
    }

    public void HidePauseMenu() {
        Time.timeScale = 1;

        var menuAnimation = gameObject.GetComponent<InAndOutAnimation>();
        menuAnimation.MoveOutAnimation();
    }
    
    // General Tween Animations
    private void OnSelectButton(GameObject target) {
        LeanTween.scale(target, new Vector3(1.3f, 1.3f, 1.3f), 0.3f).setEaseOutExpo().setIgnoreTimeScale(true);
    }
    
    //UI Animation based in button deselection
    private void OnDeselectButton(GameObject target) {
        LeanTween.scale(target, Vector3.one, 0.3f).setEaseOutExpo().setIgnoreTimeScale(true);;
    }
}
