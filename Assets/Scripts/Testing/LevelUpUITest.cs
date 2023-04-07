using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUITest : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private LevelUpUISkillCardTest leftSkillCard,
        middleSkillCard,
        rightSkillCard;

    private float fadeInSpeed = 1.5f;
    private PlayerStatus playerStatus;

    private void Awake()
    {
        Hide();
    }

    private void Start()
    {
        playerStatus = GameManager.PlayerStatus();
        playerStatus.onLevelUp += ShowLevelUpUI;
    }

    private void ShowLevelUpUI(LevelUpUISkillCardData[] skillCardDatum)
    {
        leftSkillCard.SetSkillCard(skillCardDatum[0]);
        middleSkillCard.SetSkillCard(skillCardDatum[1]);
        rightSkillCard.SetSkillCard(skillCardDatum[2]);

        StartCoroutine(FadeIn());
    }

    private void ShowLevelUpUI(SkillData[] skillDatum)
    {
        leftSkillCard.SetSkillCard(skillDatum[0]);
        middleSkillCard.SetSkillCard(skillDatum[1]);
        rightSkillCard.SetSkillCard(skillDatum[2]);

        StartCoroutine(FadeIn());
    }

    private void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.unscaledDeltaTime * fadeInSpeed;
            yield return null;
        }

        Show();
    }
}
