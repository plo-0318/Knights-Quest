using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionUITest : MonoBehaviour
{
    private void Start() { }

    public void SetResolution1920()
    {
        Screen.SetResolution(1920, 1080, false);
    }

    public void SetResolution1280()
    {
        Screen.SetResolution(1280, 720, false);
    }

    public void SetResolution720()
    {
        Screen.SetResolution(640, 360, false);
    }
}
