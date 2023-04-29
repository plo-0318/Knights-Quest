using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio References")]
public class AudioReferences : ScriptableObject
{
    [Header("Music")]
    public AudioClip musicMainMenu;
    public AudioClip musicAction1;
    public AudioClip musicAction2;
    public AudioClip musicAction3;
    public AudioClip musicBoss;
    public AudioClip musicGameOver;

    [Header("SFX Menu")]
    public AudioClip sfxMenuOpen;
    public AudioClip sfxMenuClick;
    public AudioClip sfxTick;
    public AudioClip sfxStart;

    [Header("SFX Game")]
    public AudioClip sfxPlayerHurt;
    public AudioClip sfxPlayerShieldBreak;
    public AudioClip sfxEnemyHurt;
    public AudioClip sfxEnemyHurtArrow;
    public AudioClip sfxEnemyHurtBolt;
    public AudioClip sfxEnemyHurtFireball;
    public AudioClip sfxEnemyDeath;
    public AudioClip sfxVictory;
    public AudioClip sfxDefeat;
    public AudioClip sfxBossWarning;

    [Header("SFX Collectable")]
    public AudioClip sfxPickupGem;
    public AudioClip sfxPickupBomb;
    public AudioClip sfxPickupPotion;
    public AudioClip sfxPickupShield;
    public AudioClip sfxPickupPouch;
    public AudioClip sfxBarrelBreak;

    [Header("SFX Skill")]
    public AudioClip sfxDaggerUse;
    public AudioClip sfxSwordUse1;
    public AudioClip sfxSwordUse2;
    public AudioClip sfxSwordUse3;
    public AudioClip sfxHammerUse1;
    public AudioClip sfxHammerUse2;
    public AudioClip sfxBoltUse1;
    public AudioClip sfxBoltUse2;
    public AudioClip sfxBoltUse3;
    public AudioClip sfxBoltBurst;
    public AudioClip sfxArrowUse;
    public AudioClip sfxFireballUse1;
    public AudioClip sfxFireballUse2;
    public AudioClip sfxFireballUse3;

    [Header("SFX Skill")]
    public AudioClip sfxWaterSpike;
    public AudioClip sfxLightning;
}
