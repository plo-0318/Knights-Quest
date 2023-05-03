using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionUITest : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        // text.text = Screen.currentResolution.refreshRate.ToString();
    }

    private void Start()
    {
        // string s = "";

        // Resolution[] resolutions = Screen.resolutions;

        // s += resolutions.Length.ToString();
        // s += "<br>";

        // foreach (var res in resolutions)
        // {
        //     s += (res.width.ToString() + ", " + res.height.ToString() + ", " + res.refreshRate);
        //     s += "<br>";
        // }

        // text.text = s;

        var list = WindowUIUtil.GetAvailableRefreshRates();

        string s = "";

        foreach (var i in list)
        {
            s += i.ToString() + "<br>";
        }

        text.text = s;
    }
}
