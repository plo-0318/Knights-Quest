using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerModifiersTest : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI speedText;

    [SerializeField]
    private TextMeshProUGUI healthText;

    [SerializeField]
    private TextMeshProUGUI killCountText;

    private Modifier speedMod;

    private Stat stat;
    private PlayerStat playerStat;

    private void Start()
    {
        speedMod = new Modifier(Stat.Type.SPEED, gameObject.GetInstanceID(), 0);

        playerStat = GameManager.PlayerStat();
        stat = playerStat.stat;
    }

    private void FixedUpdate()
    {
        string speedStr = Mathf.RoundToInt((stat.GetStat(Stat.Type.SPEED) / 35f)).ToString();

        speedText.text = "Speed: " + speedStr;

        healthText.text = "HP: " + stat.Health.ToString();

        killCountText.text = "Kills: " + playerStat.KillCount.ToString();
    }

    public void IncreaseSpeed()
    {
        speedMod.multiplier += 0.25f;

        stat.AddModifier(speedMod);
    }

    public void DecreaseSpeed()
    {
        speedMod.multiplier -= 0.25f;

        stat.AddModifier(speedMod);
    }

    private void OnDestroy()
    {
        stat.RemoveModifier(speedMod);
    }
}
