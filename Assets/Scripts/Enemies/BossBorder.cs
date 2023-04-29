using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossBorder
{
    private static GameObject bossBorderPrefab;
    private static GameObject _bossBorder;

    private Tilemap tileMap;
    private float minXCor,
        minYCor,
        maxXCor,
        maxYCor;

    private static BossBorder instance;

    //Checks for one instance
    public static BossBorder Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BossBorder();
            }
            return instance;
        }
    }

    private BossBorder()
    {
        bossBorderPrefab = Resources.Load<GameObject>("Misc/Boss Border");

        //Get tilemap
        GameObject tileMapObject = GameObject.Find("Ground2");

        tileMap = tileMapObject.GetComponent<Tilemap>();

        //Get bounds of tilemap
        BoundsInt bounds = tileMap.cellBounds;

        //Getting minimum and maximium world position
        Vector3 minWorldPos = tileMap.CellToWorld(bounds.min);
        Vector3 maxWorldPos = tileMap.CellToWorld(bounds.max);

        //Getting specific variables for each min and max coordinate
        minXCor = minWorldPos.x;
        maxXCor = maxWorldPos.x;
        minYCor = minWorldPos.y;
        maxYCor = maxWorldPos.y;
    }

    public static void Spawn()
    {
        if (_bossBorder != null)
        {
            Remove();
        }

        Transform player = GameManager.PlayerMovement().transform;

        Vector3 spawnPos = Instance.GetClampedPosition(player);

        _bossBorder = GameObject.Instantiate(bossBorderPrefab, spawnPos, Quaternion.identity);

        TeleportPlayerIntoBossBorder();
    }

    private static void TeleportPlayerIntoBossBorder()
    {
        if (_bossBorder == null)
        {
            return;
        }

        Transform player = GameManager.PlayerMovement().transform;

        float radius = _bossBorder.GetComponent<InvertedCircleCollider2D>().radius;

        bool playerInBossBorder =
            Vector2.Distance(player.position, _bossBorder.transform.position) < radius;

        if (playerInBossBorder)
        {
            return;
        }

        Vector3 newPos = _bossBorder.transform.position - new Vector3(0, 6f, 0);

        player.transform.position = newPos;
    }

    private Vector3 GetClampedPosition(Transform player)
    {
        //How close to wall to trigger that Player is close to edge, (5 SEEMS TO BE MOST OPTIMAL)
        float distanceThreshold = 5f;

        //Getting the position where it is okay to spawn border
        float safeMinX = minXCor + distanceThreshold;
        float safeMaxX = maxXCor - distanceThreshold;
        float safeMinY = minYCor + distanceThreshold;
        float safeMaxY = maxYCor - distanceThreshold;

        //Get the player position to make it easier to write code
        float playerPosX = player.transform.position.x;
        float playerPosY = player.transform.position.y;

        //Checks for whether the player is close to each edge of the box
        bool nearLeftEdge = playerPosX - safeMinX < distanceThreshold;
        bool nearRightEdge = safeMaxX - playerPosX < distanceThreshold;
        bool nearBottomEdge = playerPosY - safeMinY < distanceThreshold;
        bool nearTopEdge = safeMaxY - playerPosY < distanceThreshold;

        //Makes sure player position is within safe min and max
        float clampX = Mathf.Clamp(playerPosX, safeMinX, safeMaxX);
        float clampY = Mathf.Clamp(playerPosY, safeMinY, safeMaxY);

        //How much to move the border into the map
        float borderInMap = 5f;

        Vector3 spawnPos;
        //All possible checks for box
        //SpawnMaxX/MinX && SpawnMinY/MaxY are used to get the max and mins of the tilemap
        //borderInMap moves the border around to fit into the map perfectly
        if (nearTopEdge && nearRightEdge)
        {
            spawnPos = new Vector3(
                safeMaxX - borderInMap,
                safeMaxY - borderInMap,
                player.position.z
            );
        }
        else if (nearBottomEdge && nearLeftEdge)
        {
            spawnPos = new Vector3(
                safeMinX + borderInMap,
                safeMinY + borderInMap,
                player.position.z
            );
        }
        else if (nearBottomEdge && nearRightEdge)
        {
            spawnPos = new Vector3(
                safeMaxX - borderInMap,
                safeMinY + borderInMap,
                player.position.z
            );
        }
        else if (nearTopEdge && nearLeftEdge)
        {
            spawnPos = new Vector3(
                safeMinX + borderInMap,
                safeMaxY - borderInMap,
                player.position.z
            );
        }
        else if (nearLeftEdge)
        {
            spawnPos = new Vector3(safeMinX + borderInMap, clampY, player.position.z);
        }
        else if (nearRightEdge)
        {
            spawnPos = new Vector3(safeMaxX - borderInMap, clampY, player.position.z);
        }
        else if (nearBottomEdge)
        {
            spawnPos = new Vector3(clampX, safeMinY + borderInMap, player.position.z);
        }
        else if (nearTopEdge)
        {
            spawnPos = new Vector3(clampX, safeMaxY - borderInMap, player.position.z);
        }
        else
        {
            spawnPos = new Vector3(clampX, clampY, player.position.z);
        }

        return spawnPos;
    }

    private static IEnumerator BossBorderFadeOut()
    {
        var main = _bossBorder.GetComponentInChildren<ParticleSystem>().main;

        main.startColor = new Color(
            main.startColor.color.r,
            main.startColor.color.g,
            main.startColor.color.b,
            0
        );

        yield return new WaitForSeconds(3f);

        Remove();
    }

    public static void FadeOutAndRemove()
    {
        GameManager.GameSession().StartCoroutine(BossBorderFadeOut());
    }

    public static void Remove()
    {
        if (_bossBorder == null)
        {
            return;
        }

        GameObject.Destroy(_bossBorder);
    }

    public static Vector3 GetBossBorderPos()
    {
        return _bossBorder == null ? Vector3.zero : _bossBorder.transform.position;
    }

    public static GameObject bossBorder => _bossBorder;
}
