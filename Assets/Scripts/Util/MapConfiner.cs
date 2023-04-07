using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class MapConfiner : MonoBehaviour
{
    [SerializeField]
    private PolygonCollider2D cameraConfinerCollider;

    [SerializeField]
    private BoxCollider2D spawnAreaCollider;

    private void Awake() { }

    private void Start()
    {
        CinemachineDynamics cd = FindObjectOfType<CinemachineDynamics>();

        if (cd)
        {
            cd.Confine(cameraConfinerCollider);
        }

        GameManager.RegisterMapConfiner(this);
    }

    public bool InsideMap(Vector2 position)
    {
        return cameraConfinerCollider.OverlapPoint(position);
    }

    public bool InsideSpawnArea(Vector2 position)
    {
        return spawnAreaCollider.OverlapPoint(position);
    }

    public BoxCollider2D SpawnAreaCollider => spawnAreaCollider;
}
