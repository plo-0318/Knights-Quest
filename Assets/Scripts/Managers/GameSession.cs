using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    private float timer;

    [SerializeField]
    private TextMeshProUGUI timerText;

    private void Awake()
    {
        GameManager.RegisterGameSession(this);
    }

    private void Start()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (timerText)
        {
            timerText.text = GetTimeString();
        }
    }

    public float GetTime()
    {
        return timer;
    }

    public string GetTimeString()
    {
        int seconds = Mathf.RoundToInt(timer);
        int minutes = seconds / 60;
        seconds %= 60;

        string timerStr = minutes.ToString() + ":";

        if (seconds < 10)
        {
            timerStr += "0";
        }

        timerStr += seconds.ToString();

        return timerStr;
    }

    public void TEST_AddDaggerSkill()
    {
        GameManager.PlayerStat().AssignSkill(new SkillDagger());
    }

    public void TEST_AddFireballSkill()
    {
        GameManager.PlayerStat().AssignSkill(new SkillFireball());
    }

    public void TEST_AddArrowSkill()
    {
        GameManager.PlayerStat().AssignSkill(new SkillArrow());
    }

    public void TEST_AddHammerSkill()
    {
        GameManager.PlayerStat().AssignSkill(new SkillHammer());
    }
}
