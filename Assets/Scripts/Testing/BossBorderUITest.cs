using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBorderUITest : MonoBehaviour
{
    private bool hasBorder;

    private void Awake()
    {
        hasBorder = false;
    }

    public void ToggleBossBorder()
    {
        if (hasBorder)
        {
            BossBorder.Remove();
        }
        else
        {
            BossBorder.Spawn();
        }

        hasBorder = !hasBorder;
    }
}
