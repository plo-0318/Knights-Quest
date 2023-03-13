using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerModifiersTest : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI speedText;

    private Modifier speedMod;

    private Stat stat;

    private void Start()
    {
        speedMod = new Modifier(Stat.Type.speed, gameObject.GetInstanceID(), 0);

        stat = GameManager.PlayerStat().stat;
    }

    private void FixedUpdate()
    {
        string speedStr = Mathf.RoundToInt((stat.GetStat(Stat.Type.speed) / 35f)).ToString();

        speedText.text = "Speed: " + speedStr;
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
