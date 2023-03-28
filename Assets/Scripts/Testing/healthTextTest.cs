using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class healthTextTest : MonoBehaviour
{
    public TextMeshPro text;
    public SpriteRenderer background;
    public Transform actor;

    private Stat stat;

    private void Start()
    {
        //     Color color = Color.blue;
        //     string layerName = "";

        //     if (actor.TryGetComponent<PlayerStatus>(out var p))
        //     {
        //         ColorUtility.TryParseHtmlString("#34FA22", out color);
        //         layerName = "Player";
        //         stat = p.stat;
        //     }
        //     else if (actor.TryGetComponent<Enemy>(out var e))
        //     {
        //         ColorUtility.TryParseHtmlString("#FF441C", out color);
        //         layerName = "Enemy";
        //         stat = e.stat;
        //     }

        //     background.sortingLayerName = layerName;
        //     text.sortingLayerID = SortingLayer.NameToID(layerName);

        //     text.color = color;
        // }

        // private void Update()
        // {
        //     text.SetText(stat.health.ToString());
    }
}
