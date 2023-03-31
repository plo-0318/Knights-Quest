using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    protected float flySpeed = 8f;
    protected bool pickedUp;
    protected Transform destination;

    protected void Awake()
    {
        pickedUp = false;
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
        Destroy(gameObject);
    }
}
