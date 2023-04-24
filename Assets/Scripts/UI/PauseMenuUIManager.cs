using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
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

    // Public Functions

    public void ShowPauseMenu() {
        var menuAnimation = gameObject.GetComponent<InAndOutAnimation>();
        menuAnimation.MoveInAnimation();
    }

    public void HidePauseMenu() {
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
