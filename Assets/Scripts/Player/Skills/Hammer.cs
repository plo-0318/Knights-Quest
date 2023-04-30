using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private static List<GameObject> spawnedHammers = new List<GameObject>();

    private Transform hammerHolder;
    private float damage;
    private float swingSpeed;

    [SerializeField]
    GameObject onHitFx;

    private float startRotate;
    private float endRotate;
    private float currentRotation;
    private float degreeToRotate;
    private bool rotateClockwise;
    private bool persist;
    private float elapsedTime;

    public static void RemoveAllSpawnedHammers()
    {
        foreach (GameObject spawnedHammer in spawnedHammers)
        {
            GameObject.Destroy(spawnedHammer);
        }
    }

    private void Awake()
    {
        hammerHolder = transform.parent;
        persist = false;
        elapsedTime = 0.0f;

        spawnedHammers.Add(hammerHolder.gameObject);
    }

    private void Update()
    {
        Rotate();
    }

    private void OnDestroy()
    {
        spawnedHammers.Remove(hammerHolder.gameObject);

        if (persist)
        {
            GameManager.GameSession().onGameLost -= OnGameLost;
        }
    }

    private void Rotate()
    {
        if (!persist && elapsedTime > degreeToRotate)
        {
            Destroy(hammerHolder.gameObject);
        }

        float step = swingSpeed * Time.deltaTime;
        elapsedTime += step;

        float zOffset = rotateClockwise ? -step : step;

        currentRotation += zOffset;

        hammerHolder.transform.rotation = Quaternion.Euler(
            hammerHolder.transform.rotation.x,
            hammerHolder.transform.rotation.y,
            currentRotation
        );
    }

    public void Init(
        float damage,
        float degreeToRotate,
        float currentRotation,
        float rotateSpeed,
        bool clockwise,
        bool persist
    )
    {
        this.damage = damage;
        this.degreeToRotate = degreeToRotate;
        this.currentRotation = currentRotation;
        swingSpeed = rotateSpeed;
        rotateClockwise = clockwise;
        this.persist = persist;

        if (persist)
        {
            GameManager.GameSession().onGameLost += OnGameLost;
        }
    }

    private void OnGameLost()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (enemy is WalkingEnemy)
            {
                WalkingEnemy we = (WalkingEnemy)enemy;
                we.KnockBack();
            }

            enemy.Hurt(damage, onHitFx, transform.position);
        }
    }
}
