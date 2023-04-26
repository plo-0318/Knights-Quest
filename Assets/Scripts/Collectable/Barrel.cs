using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private Animator anim;
    private CollectableSpawner collectableSpawner;

    [SerializeField]
    private Transform point1,
        point2;
    private bool destroyed;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        collectableSpawner = GetComponent<CollectableSpawner>();

        destroyed = false;
    }

    private bool Broken()
    {
        return Physics2D.OverlapArea(point1.position, point2.position, LayerMask.GetMask("Skill"));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (destroyed || other.GetComponent<PlayerMovement>() != null)
        {
            return;
        }

        destroyed = true;

        // It is a skill, add logic for breaking the chest
        anim.SetTrigger("Destroy");

        SoundManager soundManager = GameManager.SoundManager();
        soundManager.PlayClip(soundManager.audioRefs.sfxBarrelBreak);

        StartCoroutine(SpawnCollectableAfterAnimation(0.4f));
    }

    private IEnumerator SpawnCollectableAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        collectableSpawner.SpawnRandomCollectables(transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
