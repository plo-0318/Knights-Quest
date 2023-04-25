using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStat : Stat
{
    public const int ITEM_PICKUP_RADIUS = 3;

    protected int _killCount;
    protected float _exp;
    protected int _level;

    ///////////////////// _EXP FORMULA /////////////////////
    private const int BASE_EXP = 100;
    private const int LINEAR_INCREMENT = 75;
    private const int EXPONENTIAL_INCREMENT = 100;

    /////////////////////////////////////////////////////

    public PlayerStat(float maxHealth, float damage, float speed, float itemPickupRadius)
    {
        NUMBER_OF_STATS = 4;

        stats = new List<float>();

        stats.Add(maxHealth);
        stats.Add(damage);
        stats.Add(speed);
        stats.Add(itemPickupRadius);

        BASE_STATS = new List<float>(stats);

        currentHealth = GetStat(MAX_HEALTH);

        InitModifiers();

        _killCount = 0;
        _exp = 0;
        _level = 0;

        //TODO: Delete this log
        // for (int i = 1; i <= 30; i++)
        // {
        //     Debug.Log("level " + i + ": " + ExpNeededToLevelUp(i));
        // }
    }

    private float ExpNeededToLevelUp(int level)
    {
        return BASE_EXP + LINEAR_INCREMENT * level + EXPONENTIAL_INCREMENT * Mathf.Pow(level, 2);
    }

    public void IncrementKillCount()
    {
        _killCount++;
    }

    public int IncreaseExp(float amount)
    {
        int levelUps = 0;

        _exp += amount;

        //TODO: DELETE
        Debug.Log("exp: " + _exp + "/" + ExpNeededToLevelUp(_level + 1));

        while (exp >= ExpNeededToLevelUp(_level + 1))
        {
            _level++;
            levelUps++;
        }

        return levelUps;
    }

    public int killCount => _killCount;
    public float exp => _exp;
    public int level => _level;
}
