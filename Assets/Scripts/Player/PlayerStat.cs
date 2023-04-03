using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStat : Stat
{
    public const int ITEM_PICKUP_SCALE = 5;

    protected float itemPickupScale;
    protected int _killCount;
    protected float _exp;
    protected int _level;

    ///////////////////// _EXP FORMULA /////////////////////
    private const int BASE_EXP = 100;
    private const int LINEAR_INCREMENT = 50;
    private const int EXPONENTIAL_INCREMENT = 25;

    /////////////////////////////////////////////////////

    public PlayerStat(
        float maxHealth,
        float damage,
        float speed,
        float prjectileSpeed = 1f,
        float scale = 1f,
        float itemPickupScale = 1f
    )
    {
        NUMBER_OF_STATS = 6;

        stats = new List<float>();

        stats.Add(maxHealth);
        stats.Add(damage);
        stats.Add(speed);
        stats.Add(prjectileSpeed);
        stats.Add(scale);
        stats.Add(itemPickupScale);

        BASE_STATS = new List<float>(stats);

        currentHealth = GetStat(MAX_HEALTH);

        _killCount = 0;
        _exp = 0;
        _level = 0;

        InitModifiers();

        //TODO: Delete this log
        // for (int i = 1; i <= 30; i++)
        // {
        //     Debug.Log(ExpNeededToLevelUp(i));
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
    public double exp => _exp;
    public int level => _level;
}
