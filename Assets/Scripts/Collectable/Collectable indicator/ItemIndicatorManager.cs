using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIndicatorManager : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private bool showIndicators;

    private void Awake()
    {
        GameManager.RegisterItemIndicatorManager(this);

        canvasGroup = GetComponent<CanvasGroup>();

        showIndicators = PlayerPrefsController.GetShowIndicators();
    }

    public void Show()
    {
        canvasGroup.alpha = 1f;
        showIndicators = true;

        ItemIndicator.ShowAllIndicators();
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        showIndicators = false;

        ItemIndicator.HideAllIndicators();
    }

    public bool ShowIndicators => showIndicators;
}
