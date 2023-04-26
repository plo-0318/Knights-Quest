using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIBossEvent : UIGameEvent
{
    [SerializeField]
    private TextMeshProUGUI bossText;

    private Coroutine playWarningSFXCoroutine;
    private float eventDuration;

    protected override void Awake()
    {
        base.Awake();

        bossText.text = "Boss";
        eventDuration = 3f;
    }

    protected override void StartEvent()
    {
        StartCoroutine(BlinkText());
        playWarningSFXCoroutine = StartCoroutine(PlayWarningSFX());
    }

    private IEnumerator BlinkText()
    {
        float blinkDuration = eventDuration;
        float currentTime = 0;
        float oscillationSpeed = 4.5f;

        Color originalColor = new Color(
            bossText.color.r,
            bossText.color.g,
            bossText.color.b,
            bossText.color.a
        );

        while (currentTime < blinkDuration)
        {
            var a = Mathf.PingPong(Time.time * oscillationSpeed, 1f);

            bossText.color = new Color(originalColor.r, originalColor.g, originalColor.b, a);

            currentTime += Time.deltaTime;
            yield return null;
        }

        bossText.color = originalColor;

        if (playWarningSFXCoroutine != null)
        {
            StopCoroutine(playWarningSFXCoroutine);
        }

        EndEvent();
    }

    private IEnumerator PlayWarningSFX()
    {
        SoundManager soundManager = GameManager.SoundManager();

        while (true)
        {
            soundManager.PlayClip(soundManager.audioRefs.sfxBossWarning);

            yield return new WaitForSeconds(0.75f);
        }
    }
}
