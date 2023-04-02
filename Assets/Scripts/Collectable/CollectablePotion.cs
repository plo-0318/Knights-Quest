using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePotion : Collectable
{
    protected override void Start()
    {
        base.Start();

        pickupSFX = soundManager.audioRefs.sfxPickupPotion;
    }

    public override void Use()
    {
        //TODO: USE LOGIC HERE

        base.Use();
    }
}
