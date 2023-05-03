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
        InitButton(continueButton, GameManager.PauseManager().HidePauseMenu);

        // Options button
        InitButton(optionsButton, NavigateToOptions);

        // Quit button
        InitButton(
            mainMenuButton,
            () =>
            {
                Debug.Log("quit...");
            }
        );
        //////// //////// //////// //////// ////////

        //////// Options Menu - Listeners ////////
        InitButton(optionsBackButton, NavigateToPause);
        InitButton(optionsResolutionButton, NavigateToResolution);
        InitButton(optionsVolumeButton, NavigateToVolume);
        //////// //////// //////// //////// ////////


        //////// Resolution Menu - Listeners ////////
        InitButton(resolutionBackButton, NavigateToOptions);
        //////// //////// //////// //////// ////////

        //////// Resolution Menu - Listeners ////////
        InitButton(volumeBackButton, NavigateToOptions);
        //////// //////// //////// //////// ////////
    }

    private void NavigateToPause()
    {
        EventSystem.current.SetSelectedGameObject(null);
        optionsScreen.SetActive(false);
        resolutionScreen.SetActive(false);
        volumeScreen.SetActive(false);

        pauseMenuScreen.SetActive(true);
    }

    private void NavigateToOptions()
    {
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenuScreen.SetActive(false);
        resolutionScreen.SetActive(false);
        volumeScreen.SetActive(false);

        optionsScreen.SetActive(true);
    }

    private void NavigateToResolution()
    {
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenuScreen.SetActive(false);
        optionsScreen.SetActive(false);
        volumeScreen.SetActive(false);

        resolutionScreen.SetActive(true);
        resolutionScreen.GetComponent<ResolutionUIManager>().Init();
    }

    private void NavigateToVolume()
    {
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenuScreen.SetActive(false);
        resolutionScreen.SetActive(false);
        optionsScreen.SetActive(false);

        volumeScreen.SetActive(true);
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

    // General Tween Animations
    private void OnSelectButton(GameObject target)
    {
        soundManager.PlayClip(
            soundManager.audioRefs.sfxMouseHover,
            SoundManager.TimedSFX.MOUSE_HOVER
        );

        LeanTween
            .scale(target, new Vector3(1.3f, 1.3f, 1.3f), 0.3f)
            .setEaseOutExpo()
            .setIgnoreTimeScale(true);
    }

    //UI Animation based in button deselection
    private void OnDeselectButton(GameObject target)
    {
        LeanTween.scale(target, Vector3.one, 0.3f).setEaseOutExpo().setIgnoreTimeScale(true);
    }

    private void InitButton(GameObject button, UnityAction onClick)
    {
        var btn = button.GetComponent<Button>();
        var btnEvents = button.GetComponent<ButtonEventHandler>();
        btn.onClick.AddListener(PlayClickSFX);
        btn.onClick.AddListener(onClick);
        btnEvents.onSelectAction.AddListener(
            delegate
            {
                OnSelectButton(button);
            }
        );
        btnEvents.onDeselectAction.AddListener(
            delegate
            {
                OnDeselectButton(button);
            }
        );
    }

    private void PlayClickSFX()
    {
        soundManager.PlayClip(soundManager.audioRefs.sfxMenuClick);
    }
}
