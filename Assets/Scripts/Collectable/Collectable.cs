using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public enum Type
    {
        GEM_GREEN,
        GEM_BLUE,
        GEM_ORANGE,
        GEM_RED,
        POTION,
        SHIELD,
        POUCH
    }

    protected const float flySpeed = 15f;
    protected bool pickedUp;
    protected Transform destination;

    protected SoundManager soundManager;
    protected AudioClip pickupSFX;

    public static HashSet<Collectable> collectables = new HashSet<Collectable>();

    protected void Awake()
    {
        pickedUp = false;

        collectables.Add(this);
    }

    protected virtual void Start()
    {
        soundManager = GameManager.SoundManager();
    }

    private void Update()
    {
        if (pickedUp)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                destination.position,
                flySpeed * Time.deltaTime
            );
        }
    }

    public void PickUp(Transform destination)
    {
        if (pickedUp)
        {
            return;
        }

        pickedUp = true;
        this.destination = destination;
    }

    public virtual void Use()
    {
        if (pickupSFX != null)
        {
            GameManager.SoundManager().PlayClip(pickupSFX);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        collectables.Remove(this);
    }

    public static void PickUpAllCollectables()
    {
        Transform absorb = FindObjectOfType<CollectableAbsorb>().transform;

        if (absorb != null)
        {
            foreach (var col in collectables)
            {
                col.PickUp(absorb);
            }
        }
    }

    public static Collectable GEM_GREEN => GameManager.GetCollectable(Type.GEM_GREEN);
    public static Collectable GEM_BLUE => GameManager.GetCollectable(Type.GEM_BLUE);
    public static Collectable GEM_ORANGE => GameManager.GetCollectable(Type.GEM_ORANGE);
    public static Collectable GEM_RED => GameManager.GetCollectable(Type.GEM_RED);
    public static Collectable POTION => GameManager.GetCollectable(Type.POTION);
    public static Collectable SHIELD => GameManager.GetCollectable(Type.SHIELD);
    public static Collectable POUCH => GameManager.GetCollectable(Type.POUCH);
}
