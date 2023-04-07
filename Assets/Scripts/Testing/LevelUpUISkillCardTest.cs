using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class LevelUpUISkillCardTest : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private Image skillIcon;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private Button selectButton;

    private int level;

    public void SetSkillCard(
        string nameText,
        Sprite iconSprite,
        string descriptionText,
        int level,
        UnityAction onSelect
    )
    {
        selectButton.onClick.RemoveAllListeners();

        this.nameText.text = nameText;
        skillIcon.sprite = iconSprite;
        this.descriptionText.text = descriptionText;
        selectButton.onClick.AddListener(onSelect);
        this.level = level;
    }

    public void SetSkillCard(LevelUpUISkillCardData data)
    {
        selectButton.onClick.RemoveAllListeners();

        this.nameText.text = data.nameText;
        skillIcon.sprite = data.iconSprite;
        this.descriptionText.text = data.descriptionText;
        this.level = data.level;
        selectButton.onClick.AddListener(data.onSelect);
    }

    public void SetSkillCard(SkillData skilldata)
    {
        selectButton.onClick.RemoveAllListeners();

        nameText.text = skilldata.DisplayName;
        skillIcon.sprite = skilldata.Sprite;
        descriptionText.text = skilldata.GetCurrentDescription();
        level = skilldata.GetCurrentLevel() + 1;
        selectButton.onClick.AddListener(skilldata.GetOnUISelect());
    }
}

public struct LevelUpUISkillCardData
{
    public string nameText;
    public Sprite iconSprite;
    public string descriptionText;
    public int level;
    public UnityAction onSelect;

    public LevelUpUISkillCardData(
        string nameText,
        Sprite iconSprite,
        string descriptionText,
        int level,
        UnityAction onSelect
    )
    {
        this.nameText = nameText;
        this.iconSprite = iconSprite;
        this.descriptionText = descriptionText;
        this.onSelect = onSelect;
        this.level = level;
    }
}
