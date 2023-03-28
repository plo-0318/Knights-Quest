using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class arrowLevelTextScr : MonoBehaviour
{
    // FOR TESTING, WILL DELETE LATER

    private TextMeshProUGUI text;
    private PlayerStatus playerStatus;
    private Dictionary<string, Skill> skills;
    private bool found;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "Arrow Level : 0";

        playerStatus = GameManager.PlayerStatus();

        found = false;
    }

    private void Update()
    {
        if (!found)
        {
            if (playerStatus.GetSkills() == null)
            {
                return;
            }
            else
            {
                found = true;
                skills = playerStatus.GetSkills();
            }
        }

        if (skills.TryGetValue("Arrow", out Skill skill))
        {
            text.text = "Arrow Level : " + skill.Level().ToString();
        }
    }
}
