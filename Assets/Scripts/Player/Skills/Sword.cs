using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private float damage;
    private Vector3 BASE_SCALE;

    [SerializeField]
    GameObject onHitFx;
    private Transform playerTransform;
    private Vector2 positionOffset;

    private Collider2D col;

    private void Awake()
    {
        BASE_SCALE = new Vector3(
            transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );

        col = GetComponent<Collider2D>();
        col.enabled = false;
    }

    private void Start()
    {
        playerTransform = GameManager.PlayerMovement().transform;
    }

    private void Update()
    {
        transform.position = playerTransform.position + (Vector3)positionOffset;
    }

    public void Init(float damage, Vector2 positionOffset, float scaleMultiplier = 1f)
    {
        this.damage = damage;
        this.positionOffset = positionOffset;

        transform.localScale = new Vector3(
            BASE_SCALE.x,
            BASE_SCALE.y * scaleMultiplier,
            BASE_SCALE.z
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Hurt(damage, onHitFx, transform.position);
        }
    }

    private void EnableCollider()
    {
        col.enabled = true;
    }

    private void DisableCollider()
    {
        col.enabled = false;
    }
}
