using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class arrowLevelTextScr : MonoBehaviour
{
    // FOR TESTING, WILL DELETE LATER

    private TextMeshProUGUI text;
    private PlayerStat playerStat;
    private Dictionary<string, Skill> skills;
    private bool found;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "Arrow Level : 0";

        playerStat = GameManager.PlayerStat();

        found = false;
    }

    private void Update()
    {
        if (!found)
        {
            if (playerStat.GetSkills() == null)
            {
                return;
            }
            else
            {
                found = true;
                skills = playerStat.GetSkills();
            }
        }

        if (skills.TryGetValue("Arrow", out Skill skill))
        {
            text.text = "Arrow Level : " + skill.Level().ToString();
        }
    }
}
