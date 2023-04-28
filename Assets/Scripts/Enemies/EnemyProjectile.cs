using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    //TODO: add code for making the projectile fly and hurting the player
    

    private Rigidbody2D rb;
    private float destroyTimer;
    private float destroyTime;

    private float speed = 10f;
    private float range = 15f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    

    private void Start()
    {
        destroyTimer = 0f;
        destroyTime = 3f;
    }

    private void Update()
    {
        if (destroyTimer >= destroyTime)
        {
            Destroy(gameObject);
        }

        destroyTimer += Time.deltaTime;
        RotateToPlayer();
        
    }



    void RotateToPlayer()
    {
        // get the angle between 2 positions
        Vector3 playerPos = GameManager.PlayerMovement().transform.position;
        Vector3 objectPos = transform.position;
        // float angleBetweenPoints = Util.GetNormalizedAngle(point1, point2);
        float angleBetweenPoints = Util.GetNormalizedAngle(objectPos, playerPos);
        // transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angleBetweenPoints - 45f));
        // rb.AddForce(transform.up * speed); //* Time.deltaTime 
        transform.rotation = Quaternion.Euler(0, 0, angleBetweenPoints);

        Vector2 direction = new Vector2(Mathf.Cos(angleBetweenPoints * Mathf.Deg2Rad), Mathf.Sin(angleBetweenPoints * Mathf.Deg2Rad));

        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out Player PlayerCollider))
        {
           // hurt the player
           GameManager.PlayerStatus().Hurt(100f); 
        }
    }
}
