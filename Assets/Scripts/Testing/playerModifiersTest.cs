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

    private PlayerStatus playerStatus;

    private void Start()
    {
        speedMod = new Modifier(Stat.SPEED, gameObject.GetInstanceID(), 0);

        playerStatus = GameManager.PlayerStatus();
    }

    private void FixedUpdate()
    {
        string speedStr = Mathf.RoundToInt((playerStatus.GetStat(Stat.SPEED) / 35f)).ToString();

        speedText.text = "Speed: " + speedStr;

        healthText.text = "HP: " + playerStatus.stat.health.ToString();

        killCountText.text = "Kills: " + playerStatus.stat.killCount.ToString();
    }

    public void IncreaseSpeed()
    {
        speedMod.multiplier += 0.25f;

        playerStatus.stat.AddModifier(speedMod);
    }

    public void DecreaseSpeed()
    {
        speedMod.multiplier -= 0.25f;

        playerStatus.stat.AddModifier(speedMod);
    }

    private void OnDestroy()
    {
        playerStatus.stat.RemoveModifier(speedMod);
    }
}
