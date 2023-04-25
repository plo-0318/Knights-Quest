using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIGameEvent : MonoBehaviour
{
    [SerializeField]
    protected GameObject uiHolder;
    protected CanvasGroup canvasGroup;

    protected event Action onEventFinish;

    protected List<Action> finishEventHandlers;

    protected float popupTime;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        finishEventHandlers = new List<Action>();

        popupTime = 0.3f;

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
        StartCoroutine(Popup());
    }

    public void PlayEvent(Action e)
    {
        AddFinishEventHandler(e);

        StartCoroutine(Popup());
    }

    protected abstract void StartEvent();

    protected virtual void EndEvent()
    {
        Hide();
        onEventFinish?.Invoke();
    }

    public void AddFinishEventHandler(Action e)
    {
        if (finishEventHandlers.Contains(e))
        {
            return;
        }

        onEventFinish += e;
        finishEventHandlers.Add(e);
    }

    private IEnumerator Popup()
    {
        RectTransform trans = uiHolder.GetComponent<RectTransform>();

        trans.localScale = Vector3.zero;
        canvasGroup.alpha = 1f;

        float elapsedTime = 0;

        while (elapsedTime < popupTime)
        {
            float t = elapsedTime / popupTime;
            float scale = Mathf.Lerp(0, 1f, t);

            trans.localScale = new Vector3(scale, scale, scale);

            elapsedTime += Time.deltaTime;

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
