using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepellentField : MonoBehaviour
{
    private float damage,
        speed;
    private bool inRange;

    [SerializeField]
    private Rigidbody2D rb;

    private float destroyTimer;
    private float destroyTime;

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
    }

    public void Init(float damage, Vector2 velocity, bool inRange = false)
    {
        this.damage = damage;
        rb.velocity = velocity;
        this.inRange = inRange;
    }
}
