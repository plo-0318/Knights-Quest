using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossBorder
{
    private static GameObject bossBorderPrefab;
    private static GameObject bossBorder;

    static BossBorder()
    {
        bossBorderPrefab = Resources.Load<GameObject>("Misc/Boss Border");
    }

    public static void Spawn()
    {
        Transform player = GameManager.PlayerMovement().transform;

        // TODO: Add logic for finding the valid spawn position here
        Vector3 spawnPos = player.position;

        bossBorder = GameObject.Instantiate(bossBorderPrefab, spawnPos, Quaternion.identity);
    }

    public static void Remove()
    {
        if (bossBorder == null)
        {
            return;
        }

        GameObject.Destroy(bossBorder);
    }
}