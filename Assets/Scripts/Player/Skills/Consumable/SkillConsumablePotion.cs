using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillConsumablePotion : SkillConsumable
{
    public SkillConsumablePotion()
    {
        name = "consumablePotion";
        _displayName = "Health Potion";
        _description = "Recover 20% of the maximum health";
        _sprite = Util.LoadSprite("skill icons/icons", "Icons_114");
    }

    public override void Use()
    {
        SoundManager soundManager = GameManager.SoundManager();

        PlayerStatus ps = GameManager.PlayerStatus();
        float maxHealth = ps.GetStat(Stat.MAX_HEALTH);

        soundManager.PlayClip(soundManager.audioRefs.sfxPickupPotion);
        ps.Heal(maxHealth * 0.2f);
    }
}
