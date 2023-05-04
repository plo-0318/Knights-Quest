using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static class UIUtil
{
    private static SoundManager soundManager;

    static UIUtil()
    {
        soundManager = GameManager.SoundManager();
    }

    public static void InitButton(
        GameObject buttonGameObject,
        UnityAction onClick,
        float onSelectScale = 1.3f,
        float onSelectScaleDuration = 0.3f
    )
    {
        var btn = buttonGameObject.GetComponent<Button>();

        btn.onClick.AddListener(PlayClickSFX);
        btn.onClick.AddListener(onClick);

        if (buttonGameObject.TryGetComponent<ButtonEventHandler>(out var btnEvents))
        {
            btnEvents.onSelectAction.AddListener(
                delegate
                {
                    OnSelectButton(buttonGameObject, onSelectScale, onSelectScaleDuration);
                }
            );
            btnEvents.onDeselectAction.AddListener(
                delegate
                {
                    OnDeselectButton(buttonGameObject, onSelectScaleDuration);
                }
            );
        }
    }

    private static void OnSelectButton(GameObject target, float scale, float duration)
    {
        soundManager.PlayClip(
            soundManager.audioRefs.sfxMouseHover,
            SoundManager.TimedSFX.MOUSE_HOVER
        );

        LeanTween
            .scale(target, new Vector3(scale, scale, scale), duration)
            .setEaseOutExpo()
            .setIgnoreTimeScale(true);
    }

    //UI Animation based in button deselection
    private static void OnDeselectButton(GameObject target, float duration)
    {
        LeanTween.scale(target, Vector3.one, duration).setEaseOutExpo().setIgnoreTimeScale(true);
    }

    private static void PlayClickSFX()
    {
        soundManager.PlayClip(soundManager.audioRefs.sfxMenuClick);
    }
}