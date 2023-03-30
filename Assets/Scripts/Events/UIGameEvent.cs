using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIGameEvent : MonoBehaviour
{
    protected CanvasGroup canvasGroup;

    protected event Action onEventFinish;

    protected List<Action> finishEventHandlers;

    protected float fadeInSpeed;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        finishEventHandlers = new List<Action>();

        fadeInSpeed = 0.8f;

        Hide();
    }

    protected void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    protected void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void PlayEvent()
    {
        StartCoroutine(FadeIn());
    }

    protected abstract void StartEvent();

    protected virtual void EndEvent()
    {
        Hide();
        onEventFinish?.Invoke();
    }

    public void AddFinishEventHandler(Action e)
    {
        onEventFinish += e;
        finishEventHandlers.Add(e);
    }

    private IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * fadeInSpeed;
            yield return null;
        }

        Show();
        StartEvent();
    }

    private void OnDestroy()
    {
        foreach (Action e in finishEventHandlers)
        {
            onEventFinish -= e;
        }
    }
}
