using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class hammerLevelTextScr : MonoBehaviour
{
    // FOR TESTING, WILL DELETE LATER

    private TextMeshProUGUI text;
    private PlayerStat playerStat;
    private Dictionary<string, Skill> skills;
    private bool found;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "Hammer Level : 0";

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

        if (skills.TryGetValue("Hammer", out Skill skill))
        {
            text.text = "Hammer Level : " + skill.Level().ToString();
        }
    }
}
