using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsUIManager : MonoBehaviour
{
    [Header("Item Indicator")]
    [SerializeField]
    private Toggle itemIndicatorToggle;

    private void Awake()
    {
        itemIndicatorToggle.onValueChanged.AddListener(HandleItemIndicatorToggle);
        itemIndicatorToggle.isOn = PlayerPrefsController.GetShowIndicators();
    }

    private void HandleItemIndicatorToggle(bool show)
    {
        // Save the setting
        PlayerPrefsController.SetShowIndicators(show);

        // If in main menu, abort
        if (GameManager.ItemIndicatorManager() == null)
        {
            return;
        }

        // If in game, show/hide the item indicators
        if (show)
        {
            GameManager.ItemIndicatorManager().Show();
        }
        else
        {
            GameManager.ItemIndicatorManager().Hide();
        }
    }
}
