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
        PlayerStatus ps = GameManager.PlayerStatus();
        float maxHealth = ps.GetStat(Stat.MAX_HEALTH);

        ps.Heal(maxHealth * 0.3f);

        base.Use();
    }
}
