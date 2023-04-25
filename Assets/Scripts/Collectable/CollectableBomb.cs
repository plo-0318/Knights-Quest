using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBomb : Collectable
{
    protected override void Start()
    {
        base.Start();

        pickupSFX = soundManager.audioRefs.sfxPickupBomb;
    }

    public override void Use()
    {
        GameManager.GameSession().KillAllEnemies(true);

        base.Use();
    }
}
