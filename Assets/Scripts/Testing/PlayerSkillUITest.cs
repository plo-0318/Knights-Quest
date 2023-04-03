using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerSkillUITest : MonoBehaviour
{
    private class SkillUI
    {
        private Button levelUpButton;
        private TextMeshProUGUI lvText;

        public SkillUI(Button levelUpButton, TextMeshProUGUI lvText)
        {
            this.levelUpButton = levelUpButton;
            this.lvText = lvText;
        }

        public void AddButtonEvent(UnityAction e)
        {
            levelUpButton.onClick.AddListener(e);
        }

        public void SetText(string text)
        {
            lvText.text = text;
        }

        public void SetButtonInteractable(bool interactable)
        {
            levelUpButton.interactable = interactable;
        }

        public static SkillUI NewSkillUI(GameObject obj, UnityAction e)
        {
            Button btn = obj.GetComponentInChildren<Button>();
            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();

            var skillUI = new SkillUI(btn, text);
            skillUI.AddButtonEvent(e);

            return skillUI;
        }
    }

    private Dictionary<string, Skill> skills;
    private Dictionary<string, SkillUI> skillUIs;

    [SerializeField]
    private GameObject sword,
        dagger,
        field,
        hammer,
        arrow,
        fireball;

    private void Awake()
    {
        skillUIs = new Dictionary<string, SkillUI>();

        Init();
    }

    private void Start()
    {
        skills = GameManager.PlayerStatus().GetSkills();
        AddClickSound();

        UpdateText();

        GameManager.GameSession().onGameLost += DisableButtons;
    }

    private void Init()
    {
        skillUIs.Add("dagger", SkillUI.NewSkillUI(dagger, TEST_AddDaggerSkill));
        skillUIs.Add("field", SkillUI.NewSkillUI(field, TEST_AddFieldSkill));
        skillUIs.Add("arrow", SkillUI.NewSkillUI(arrow, TEST_AddArrowSkill));
        skillUIs.Add("fireball", SkillUI.NewSkillUI(fireball, TEST_AddFireballSkill));
        skillUIs.Add("hammer", SkillUI.NewSkillUI(hammer, TEST_AddHammerSkill));
        skillUIs.Add("sword", SkillUI.NewSkillUI(sword, TEST_AddSwordSkill));
    }

    private void AddClickSound()
    {
        foreach (var kvp in skillUIs)
        {
            var soundManager = GameManager.SoundManager();

            kvp.Value.AddButtonEvent(() =>
            {
                soundManager.PlayClip(soundManager.audioRefs.sfxMenuClick);
            });
        }
    }

    private void UpdateText()
    {
        foreach (KeyValuePair<string, SkillUI> kvp in skillUIs)
        {
            if (skills.TryGetValue(kvp.Key, out var skill))
            {
                kvp.Value.SetText(skill.Level().ToString());
            }
            else
            {
                kvp.Value.SetText("0");
            }
        }
    }

    private void DisableButtons()
    {
        foreach (var kvp in skillUIs)
        {
            kvp.Value.SetButtonInteractable(false);
        }
    }

    public void TEST_AddSwordSkill()
    {
        GameManager.PlayerStatus().AssignSkill(new SkillSword());

        UpdateText();
    }

    public void TEST_AddDaggerSkill()
    {
        GameManager.PlayerStatus().AssignSkill(new SkillDagger());

        UpdateText();
    }

    public void TEST_AddFireballSkill()
    {
        Debug.LogError("Fireball not implemented yet");

        // GameManager.PlayerStatus().AssignSkill(new SkillFireball());

        UpdateText();
    }

    public void TEST_AddArrowSkill()
    {
        Debug.LogError("Arrow not implemented yet");

        // GameManager.PlayerStatus().AssignSkill(new SkillArrow());

        UpdateText();
    }

    public void TEST_AddHammerSkill()
    {
        GameManager.PlayerStatus().AssignSkill(new SkillHammer());

        UpdateText();
    }

    public void TEST_AddFieldSkill()
    {
        GameManager.PlayerStatus().AssignSkill(new SkillField());

        UpdateText();
    }
}
