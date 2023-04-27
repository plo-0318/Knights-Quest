using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpUIManager : MonoBehaviour 
{
    [SerializeField] private GameObject skillCard1;
    [SerializeField] private UnityEvent onClickSkillCard1;
    [SerializeField] private GameObject skillCard2;
    [SerializeField] private UnityEvent onClickSkillCard2;
    [SerializeField] private GameObject skillCard3;
    [SerializeField] private UnityEvent onClickSkillCard3;

    // Start is called before the first frame update
    void Start()
    {
        // Set Listeners to the buttons
        var skillButton1 = skillCard1.GetComponent<Button>();
        var skillButtonEvents1 = skillCard1.GetComponent<ButtonEventHandler>();
        skillButton1.onClick.AddListener(delegate { onClickSkillCard1.Invoke(); });
        skillButtonEvents1.onSelectAction.AddListener(delegate { OnSelectButton(skillCard1); });
        skillButtonEvents1.onDeselectAction.AddListener(delegate { OnDeselectButton(skillCard1); });
        
        var skillButton2 = skillCard2.GetComponent<Button>();
        var skillButtonEvents2 = skillCard2.GetComponent<ButtonEventHandler>();
        skillButton2.onClick.AddListener(delegate { onClickSkillCard2.Invoke(); });
        skillButtonEvents2.onSelectAction.AddListener(delegate { OnSelectButton(skillCard2); });
        skillButtonEvents2.onDeselectAction.AddListener(delegate { OnDeselectButton(skillCard2); });
        
        var skillButton3 = skillCard3.GetComponent<Button>();
        var skillButtonEvents3 = skillCard3.GetComponent<ButtonEventHandler>();
        skillButton3.onClick.AddListener(delegate { onClickSkillCard3.Invoke(); });
        skillButtonEvents3.onSelectAction.AddListener(delegate { OnSelectButton(skillCard3); });
        skillButtonEvents3.onDeselectAction.AddListener(delegate { OnDeselectButton(skillCard3); });
    }

    // Update is called once per frame
    void Update()
    {
        
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
