using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class PauseMenuUIManager : MonoBehaviour
{
    [Header("Backdrop")]
    [SerializeField]
    private Image backdrop;

    [Header("Pause Screens")]
    [SerializeField]
    private GameObject pauseMenuScreen;

    [SerializeField]
    private GameObject optionsScreen;

    [SerializeField]
    private GameObject resolutionScreen;

    [SerializeField]
    private GameObject mainMenuWarningScreen;

    [SerializeField]
    private GameObject volumeScreen;

    [Header("Pause Menu Buttons")]
    [SerializeField]
    private GameObject continueButton;

    [SerializeField]
    private UnityEvent onClickContinue;

    [SerializeField]
    private GameObject optionsButton;

    [SerializeField]
    private GameObject mainMenuButton;

    [SerializeField]
    private UnityEvent onClickQuit;

    [Header("Options Menu Buttons")]
    [SerializeField]
    private GameObject optionsResolutionButton;

    [SerializeField]
    private GameObject optionsVolumeButton;

    [SerializeField]
    private GameObject optionsBackButton;

    [Header("Resolution Menu Buttons")]
    [SerializeField]
    private GameObject resolutionBackButton;

    [Header("Volume Menu Buttons")]
    [SerializeField]
    private GameObject volumeBackButton;

    [Header("Main Menu Warning Buttons")]
    [SerializeField]
    private GameObject mainMenuConfirmButton;

    [SerializeField]
    private GameObject mainMenuBackButton;

    [Header("Pause Menu Skills")]
    [SerializeField]
    private GameObject[] combatSkills;

    [SerializeField]
    private GameObject[] utilitySkills;

    private PlayerStatus playerStatus;

    private SoundManager soundManager;

    private void Awake()
    {
        GameManager.RegisterPauseMenuUIManager(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setup Player Status
        playerStatus = GameManager.PlayerStatus();
        soundManager = GameManager.SoundManager();

        //////// Pause Menu - Listeners ////////
        // Continue button
        UIUtil.InitButton(continueButton, GameManager.PauseManager().HidePauseMenu);

        // Options button
        UIUtil.InitButton(optionsButton, NavigateToOptions);

        // Quit button
        UIUtil.InitButton(mainMenuButton, NavigateToMainMenuWarning);
        //////// //////// //////// //////// ////////

        //////// Options Menu - Listeners ////////
        UIUtil.InitButton(optionsBackButton, NavigateToPause);
        UIUtil.InitButton(optionsResolutionButton, NavigateToResolution);
        UIUtil.InitButton(optionsVolumeButton, NavigateToVolume);
        //////// //////// //////// //////// ////////


        //////// Resolution Menu - Listeners ////////
        UIUtil.InitButton(resolutionBackButton, NavigateToOptions);
        //////// //////// //////// //////// ////////

        //////// Resolution Menu - Listeners ////////
        UIUtil.InitButton(volumeBackButton, NavigateToOptions);
        //////// //////// //////// //////// ////////

        //////// Main Menu - Listeners ////////
        UIUtil.InitButton(mainMenuConfirmButton, NavigateToMainMenu);
        UIUtil.InitButton(mainMenuBackButton, NavigateToPause);
        //////// //////// //////// //////// ////////
    }

    private void CloseAllSubMenus()
    {
        EventSystem.current.SetSelectedGameObject(null);
        optionsScreen.SetActive(false);
        resolutionScreen.SetActive(false);
        volumeScreen.SetActive(false);
        pauseMenuScreen.SetActive(false);
        mainMenuWarningScreen.SetActive(false);
    }

    private void NavigateToMainMenu()
    {
        GameManager.GameSession().ResumeGame();
        GameManager.LoadScene("Main Menu", true);
    }

    private void NavigateToPause()
    {
        CloseAllSubMenus();

        pauseMenuScreen.SetActive(true);
    }

    private void NavigateToOptions()
    {
        CloseAllSubMenus();

        optionsScreen.SetActive(true);
    }

    private void NavigateToResolution()
    {
        CloseAllSubMenus();

        resolutionScreen.SetActive(true);
        resolutionScreen.GetComponent<ResolutionUIManager>().Init();
    }

    private void NavigateToVolume()
    {
        CloseAllSubMenus();

        volumeScreen.SetActive(true);
    }

    private void NavigateToMainMenuWarning()
    {
        CloseAllSubMenus();

        mainMenuWarningScreen.SetActive(true);
    }

    private void UpdateSkillUI()
    {
        var combatSkillData = playerStatus.GetSkillDatum().Where(c => c.Type == "ATTACK").ToArray();

        for (var i = 0; i < combatSkills.Length; i++)
        {
            var skillData = (combatSkillData.Length > i) ? combatSkillData[i] : null;
            var currentSkill = combatSkills[i];

            var skillTitle = currentSkill.transform
                .Find("SkillTitle")
                .gameObject.GetComponent<TextMeshProUGUI>();
            var skillDescription = currentSkill.transform
                .Find("SkillDescription")
                .gameObject.GetComponent<TextMeshProUGUI>();
            skillTitle.text = (skillData != null) ? skillData.DisplayName : "";
            skillDescription.text = (skillData != null) ? skillData.Description : "";

            var skillLevelPlaceholder = currentSkill.transform
                .Find("Skill/LevelPlaceholder")
                .gameObject.GetComponent<Image>();
            var skillIcon = currentSkill.transform
                .Find("Skill/SkillIcon")
                .gameObject.GetComponent<Image>();
            var skillLevelLabel = currentSkill.transform
                .Find("Skill/LevelLabel")
                .GetComponent<TextMeshProUGUI>();
            skillLevelPlaceholder.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            skillIcon.sprite = (skillData != null) ? skillData.Sprite : null;
            skillIcon.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            skillLevelLabel.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            if (skillData != null)
                skillLevelLabel.text = skillData.GetCurrentLevel().ToString();
        }

        var utilitySkillData = playerStatus
            .GetSkillDatum()
            .Where(c => c.Type == "UTILITY")
            .ToArray();

        for (var i = 0; i < utilitySkills.Length; i++)
        {
            var skillData = (utilitySkillData.Length > i) ? utilitySkillData[i] : null;
            var currentSkill = utilitySkills[i];

            var skillTitle = currentSkill.transform
                .Find("SkillTitle")
                .gameObject.GetComponent<TextMeshProUGUI>();
            var skillDescription = currentSkill.transform
                .Find("SkillDescription")
                .gameObject.GetComponent<TextMeshProUGUI>();
            skillTitle.text = (skillData != null) ? skillData.DisplayName : "";
            skillDescription.text = (skillData != null) ? skillData.Description : "";

            var skillLevelPlaceholder = currentSkill.transform
                .Find("Skill/LevelPlaceholder")
                .gameObject.GetComponent<Image>();
            var skillIcon = currentSkill.transform
                .Find("Skill/SkillIcon")
                .gameObject.GetComponent<Image>();
            var skillLevelLabel = currentSkill.transform
                .Find("Skill/LevelLabel")
                .GetComponent<TextMeshProUGUI>();
            skillLevelPlaceholder.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            skillIcon.sprite = (skillData != null) ? skillData.Sprite : null;
            skillIcon.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            skillLevelLabel.color = new Color(255, 255, 255, (skillData != null) ? 1f : 0f);
            if (skillData != null)
                skillLevelLabel.text = skillData.GetCurrentLevel().ToString();
        }
    }

    // Public Functions
    public void ShowPauseMenu()
    {
        // Time.timeScale = 0;

        UpdateSkillUI();

        var menuAnimation = gameObject.GetComponent<InAndOutAnimation>();
        menuAnimation.MoveInAnimation();

        backdrop.gameObject.SetActive(true);
        AnimationUtil.Fade(backdrop, 0.25f, 0, 150f / 255f, false);
    }

    public void HidePauseMenu(Action callback)
    {
        // Time.timeScale = 1;

        NavigateToPause();

        var menuAnimation = gameObject.GetComponent<InAndOutAnimation>();
        menuAnimation.MoveOutAnimation(0f, callback);

        AnimationUtil.Fade(
            backdrop,
            0.25f,
            150f / 255f,
            0,
            false,
            () =>
            {
                backdrop.gameObject.SetActive(false);
            }
        );
    }
}
