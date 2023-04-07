using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableGem : Collectable
{
    [SerializeField]
    private float expAmount = 15f;

    protected override void Start()
    {
        base.Start();

        pickupSFX = soundManager.audioRefs.sfxPickupGem;
    }

    protected override void PlayPickupSFX()
    {
        GameManager.SoundManager().PlayClip(pickupSFX, SoundManager.TimedSFX.GEM);
    }

    public override void Use()
    {
        //TODO: USE LOGIC HERE

        base.Use();
    }
}
