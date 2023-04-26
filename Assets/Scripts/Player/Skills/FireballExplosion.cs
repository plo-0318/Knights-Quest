using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballExplosion : MonoBehaviour
{
    private Collider2D col;
    private float damage;
    private GameObject fieldPrefab;

    private void Awake()
    {
        col = GetComponent<Collider2D>();

        fieldPrefab = null;

        DisableCollider();
    }

    private void Start()
    {
        SoundManager soundManager = GameManager.SoundManager();
        soundManager.PlayClip(soundManager.audioRefs.sfxEnemyHurtFireball);
    }

    private void EnableCollider()
    {
        col.enabled = true;

        if (fieldPrefab == null)
        {
            return;
        }

        FireballField field = Instantiate(
                fieldPrefab,
                transform.position,
                Quaternion.identity,
                GameManager.GameSession().skillParent
            )
            .GetComponent<FireballField>();

        field.Init(damage * 0.2f);
    }

    private void DisableCollider()
    {
        col.enabled = false;
    }

    public void Init(float damage, GameObject fieldPrefab)
    {
        this.damage = damage;
        this.fieldPrefab = fieldPrefab;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Hurt(damage);
        }
    }
}
