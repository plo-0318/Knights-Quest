using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float aliveTime;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        RotateToPlayer();
    }

    private void Update()
    {
        if (aliveTime <= 0)
        {
            Destroy(gameObject);
        }

        aliveTime -= Time.deltaTime;
    }

    private void RotateToPlayer()
    {
        // get the angle between 2 positions
        Vector3 playerPos = GameManager.PlayerMovement().transform.position;
        Vector3 objectPos = transform.position;

        float angleBetweenPoints = Util.GetNormalizedAngle(objectPos, playerPos);

        transform.rotation = Quaternion.Euler(0, 0, angleBetweenPoints);

        Vector2 direction = new Vector2(
            Mathf.Cos(angleBetweenPoints * Mathf.Deg2Rad),
            Mathf.Sin(angleBetweenPoints * Mathf.Deg2Rad)
        );

        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerCollider>() != null)
        {
            Vector2 knockbackDirection =
                GameManager.PlayerMovement().transform.position - transform.position;

            GameManager.PlayerStatus().Hurt(damage, knockbackDirection.normalized);

            Destroy(gameObject);
        }
    }
}
