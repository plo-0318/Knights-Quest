using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BorderChecker : MonoBehaviour
{
    private static BossBorder bossBorder;

    public static void Start()
    {
        bossBorder = BossBorder.Instance;
    }

    public static void Update(Transform playerTransform)
    {
        BossBorder.Spawn();
    }
}
