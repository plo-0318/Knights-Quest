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
    public AudioClip sfxEnemyHurt;
    public AudioClip sfxEnemyDeath;
    public AudioClip sfxExpPickup;
    public AudioClip sfxVictory;
    public AudioClip sfxDefeat;
    public AudioClip sfxBossWarning;

    [Header("SFX Skill")]
    public AudioClip sfxDaggerUse;
    public AudioClip sfxSwordUse;
}