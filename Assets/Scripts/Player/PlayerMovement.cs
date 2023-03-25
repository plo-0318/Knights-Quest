using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IAnimatable
{
    private GatherInput gatherInput;
    private Rigidbody2D rb;
    private Collider2D col;

    private PlayerStatus playerStatus;

    private Vector3 BASE_LOCALSCALE;

    [SerializeField]
    private float knockBackForce = 5f;

    [SerializeField]
    private float knockBackDuration = 0.15f;

    private bool canMove;

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
        col = GetComponent<Collider2D>();
        playerStatus = GetComponent<PlayerStatus>();

        BASE_LOCALSCALE = new Vector3(
            transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );

        canMove = true;

        playerStatus.onPlayerDeath += handlePlayerDeath;
    }

    private void Update() { }

    private void FixedUpdate()
    {
        Flip();
        Move();
    }

    private void OnDestroy()
    {
        playerStatus.onPlayerDeath -= handlePlayerDeath;
    }

    private void Move()
    {
        if (!canMove)
        {
            return;
        }

        Vector2 movement = new Vector2(gatherInput.moveValueX, gatherInput.moveValueY).normalized;

        rb.velocity = movement * Time.deltaTime * playerStatus.GetStat(Stat.SPEED);
    }

    private void Flip()
    {
        if (!canMove)
        {
            return;
        }

        bool hasHorizontalSpeed = Mathf.Abs(rb.velocity.x) >= Mathf.Epsilon;

        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x) * BASE_LOCALSCALE.x, 1f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            float damage = enemy.GetStat(Stat.DAMAGE);
            Vector2 direction = transform.position - enemy.transform.position;

            playerStatus.Hurt(damage, direction);
        }
    }

    public void KnockBack(Vector2 direction)
    {
        canMove = false;

        rb.velocity = direction * knockBackForce;

        StartCoroutine(RecoverFromKnockBack(knockBackDuration));
    }

    private IEnumerator RecoverFromKnockBack(float time)
    {
        yield return new WaitForSeconds(time);

        rb.velocity = Vector2.zero;

        if (!playerStatus.IsDead)
        {
            canMove = true;
        }
    }

    private void handlePlayerDeath()
    {
        canMove = false;
        col.enabled = false;
    }

    public bool IsDead() => playerStatus.IsDead;

    public bool IsIdle() => !canMove ? true : rb.velocity.magnitude <= Mathf.Epsilon;

    public bool IsMoving() => !IsIdle();

    public void TEST_knockback()
    {
        KnockBack(Vector2.down);
    }
}
