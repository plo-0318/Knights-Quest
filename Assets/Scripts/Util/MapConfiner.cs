using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class MapConfiner : MonoBehaviour
{
    private PolygonCollider2D col;
    private BoxCollider2D boxCol;

    private void Awake()
    {
        col = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        CinemachineDynamics cd = FindObjectOfType<CinemachineDynamics>();

        if (cd)
        {
            cd.Confine(col);
        }

        GameManager.RegisterMapConfiner(this);
    }

    public bool InsideMap(Vector2 position)
    {
        return col.OverlapPoint(position);
    }

    public bool InsideSpawnArea(Vector2 position)
    {
        return boxCol.OverlapPoint(position);
    }
}
