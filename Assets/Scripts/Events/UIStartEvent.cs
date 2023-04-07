using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIStartEvent : UIGameEvent
{
    [SerializeField]
    private TextMeshProUGUI countDownText;

    protected override void Awake()
    {
        base.Awake();

        countDownText.text = "Ready";
    }

    protected override void StartEvent()
    {
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        SoundManager soundManager = GameManager.SoundManager();

        yield return new WaitForSeconds(1.5f);

        soundManager.PlayClip(soundManager.audioRefs.sfxTick);
        countDownText.text = "3";

        yield return new WaitForSeconds(1f);

        soundManager.PlayClip(soundManager.audioRefs.sfxTick);
        countDownText.text = "2";

        yield return new WaitForSeconds(1f);

        soundManager.PlayClip(soundManager.audioRefs.sfxTick);
        countDownText.text = "1";

        yield return new WaitForSeconds(1f);

        soundManager.PlayClip(soundManager.audioRefs.sfxStart);
        countDownText.text = "Start";

        yield return new WaitForSeconds(1f);

        EndEvent();
    }
}
