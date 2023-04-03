using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePouch : Collectable
{
    protected override void Start()
    {
        base.Start();

        pickupSFX = soundManager.audioRefs.sfxPickupPouch;
    }

    public override void Use()
    {
        Collectable.PickUpAllCollectables();

        base.Use();
    }
}
