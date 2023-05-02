using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpUIManager : MonoBehaviour {
    [Header("Skill Cards")]
    [SerializeField] private GameObject skillCard1;
    [SerializeField] private GameObject skillCard2;
    [SerializeField] private GameObject skillCard3;

    // Player Status
    private PlayerStatus playerStatus;
    
    // Start is called before the first frame update
    void Start()
    {
        playerStatus = GameManager.PlayerStatus();
        playerStatus.onLevelUp += SetSkillCards;
        
        var skillButtonEvents1 = skillCard1.GetComponent<ButtonEventHandler>();
        skillButtonEvents1.onSelectAction.AddListener(delegate { OnSelectButton(skillCard1); });
        skillButtonEvents1.onDeselectAction.AddListener(delegate { OnDeselectButton(skillCard1); });
        
        var skillButtonEvents2 = skillCard2.GetComponent<ButtonEventHandler>();
        skillButtonEvents2.onSelectAction.AddListener(delegate { OnSelectButton(skillCard2); });
        skillButtonEvents2.onDeselectAction.AddListener(delegate { OnDeselectButton(skillCard2); });

        var skillButtonEvents3 = skillCard3.GetComponent<ButtonEventHandler>();
        skillButtonEvents3.onSelectAction.AddListener(delegate { OnSelectButton(skillCard3); });
        skillButtonEvents3.onDeselectAction.AddListener(delegate { OnDeselectButton(skillCard3); });
        
    }

    private void SetSkillCards(SkillData[] skillDatum) 
    {
        for (var i = 0; i < skillDatum.Length; i++) {
            var currentSkillCard = i switch {
                0 => skillCard1,
                1 => skillCard2,
                2 => skillCard3,
                _ => skillCard1
            };

            var skillTitle = currentSkillCard.transform.Find("SkillTitle").gameObject.GetComponent<TextMeshProUGUI>();
            skillTitle.text = skillDatum[i].DisplayName;

            var skillLevel = currentSkillCard.transform.Find("SkillLevel").gameObject.GetComponent<TextMeshProUGUI>();
            skillLevel.text = (skillDatum[i].GetCurrentLevel()+1).ToString();
            
            var skillPicture = currentSkillCard.transform.Find("SkillPicture").gameObject.GetComponent<Image>();
            skillPicture.sprite = skillDatum[i].Sprite;
            
            var skillDescription = currentSkillCard.transform.Find("SkillDescription").gameObject.GetComponent<TextMeshProUGUI>();
            skillDescription.text = skillDatum[i].GetCurrentDescription();
            
            var skillButton = currentSkillCard.GetComponent<Button>();
            skillButton.onClick.AddListener(skillDatum[i].GetOnUISelect());
        }
    }

    // General Tween Animations
    private void OnSelectButton(GameObject target) {
        LeanTween.scale(target, new Vector3(1.1f, 1.1f, 1.1f), 0.3f).setEaseOutExpo().setIgnoreTimeScale(true);;
    }
    
    //UI Animation based in button deselection
    private void OnDeselectButton(GameObject target) {
        LeanTween.scale(target, Vector3.one, 0.3f).setEaseOutExpo().setIgnoreTimeScale(true);;
    }
}
