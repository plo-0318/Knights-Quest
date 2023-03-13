using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GatherInput gatherInput;
    private Rigidbody2D rb;

    [SerializeField]
    private Stat stat;

    private void Awake()
    {
        CinemachineDynamics cd = FindObjectOfType<CinemachineDynamics>();

        if (cd)
        {
            cd.Follow(transform);
        }

        GameManager.RegisterPlayerMovement(this);
    }

    private void Start()
    {
        gatherInput = GetComponent<GatherInput>();
        rb = GetComponent<Rigidbody2D>();
        stat = GetComponent<PlayerStat>().stat;
    }

    private void Update() { }

    private void FixedUpdate()
    {
        // Debug.Log(rb.velocity);
        Move();
    }

    private void Move()
    {
        Vector2 movement = new Vector2(gatherInput.moveValueX, gatherInput.moveValueY).normalized;

        rb.velocity = movement * Time.deltaTime * stat.GetStat(Stat.Type.speed);
    }
}
