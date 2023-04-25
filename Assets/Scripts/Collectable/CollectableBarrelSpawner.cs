using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollectableBarrelSpawner : MonoBehaviour
{
    private GameObject barrelPref;
    private Transform player;

    [SerializeField]
    private float minRadius = 10f;

    [SerializeField]
    private float maxRadius = 15f;

    [SerializeField]
    private float spawnInterval = 25f;
    private float timer;

    private GameSession gameSession;

    private void Awake()
    {
        barrelPref = Resources.Load<GameObject>("collectables/Barrel");

        timer = 0;
    }

    private void Start()
    {
        player = GameManager.PlayerMovement().transform;
        gameSession = GameManager.GameSession();
    }

    private void FixedUpdate()
    {
        if (!gameSession.TimerTicking)
        {
            return;
        }

        if (timer >= spawnInterval)
        {
            SpawnBarrel();
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void SpawnBarrel()
    {
        Vector3 spawnPos = CreateSpawnPos();

        if (IsWithinBounds(spawnPos))
        {
            Instantiate(barrelPref, spawnPos, Quaternion.identity);
        }
        else
        {
            SpawnBarrel();
        }
    }

    private bool IsWithinBounds(Vector3 position)
    {
        return GameManager.MapConfiner().InsideSpawnArea(position);
    }

    private Vector3 CreateSpawnPos()
    {
        float radius = Random.Range(minRadius, maxRadius);
        Vector2 randomOffset = Random.insideUnitCircle * radius;

        Vector3 spawnPos = new Vector3(
            player.position.x + randomOffset.x,
            player.position.y + randomOffset.y,
            player.position.z
        );

        return spawnPos;
    }
}
