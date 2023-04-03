using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableShield : Collectable
{
    protected override void Start()
    {
        base.Start();

        pickupSFX = soundManager.audioRefs.sfxPickupShield;
    }

    public override void Use()
    {
        //TODO: USE LOGIC HERE

        base.Use();
    }
}
