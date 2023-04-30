using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IAnimatable
{
    private GatherInput gatherInput;
    private Rigidbody2D rb;

    private PlayerStatus playerStatus;

    private Vector3 BASE_LOCALSCALE;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Collider2D playerCollider;

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
        playerStatus = GetComponent<PlayerStatus>();

        BASE_LOCALSCALE = new Vector3(
            transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );

        canMove = true;

        GameManager.GameSession().onGameLost += HandlePlayerDeath;
        GameManager.GameSession().onGameWon += HandlePlayerDeath;
    }

    private void Update() { }

    private void FixedUpdate()
    {
        Flip();
        Move();
    }

    private void OnDestroy()
    {
        GameManager.GameSession().onGameLost -= HandlePlayerDeath;
        GameManager.GameSession().onGameWon -= HandlePlayerDeath;
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
            spriteRenderer.gameObject.transform.localScale = new Vector2(
                Mathf.Sign(rb.velocity.x) * BASE_LOCALSCALE.x,
                1f
            );
        }
    }

    public void KnockBack(Vector2 direction)
    {
        canMove = false;

        rb.velocity = direction.normalized * knockBackForce;

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

    private void HandlePlayerDeath()
    {
        rb.velocity = Vector2.zero;
        canMove = false;
        playerCollider.enabled = false;
    }

    public bool IsDead() => playerStatus.IsDead;

    public bool IsIdle() => !canMove ? true : rb.velocity.magnitude <= Mathf.Epsilon;

    public bool IsMoving() => !IsIdle();

    public Vector3 GetIconSpawnPosition()
    {
        return transform.position + new Vector3(0, 0.8f, 0);
    }

    public Transform GetSpawnParent()
    {
        return transform;
    }

    public SpriteRenderer SpriteRender => spriteRenderer;
    public Collider2D PlayerCollider => playerCollider;

    public Vector3 MousePos => Camera.main.ScreenToWorldPoint(gatherInput.mousePos);

    public void TEST_knockback()
    {
        KnockBack(Vector2.down);
    }
}
