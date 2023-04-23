using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Util
{
    public static IDictionary<K, V> FilterDictionary<K, V>(
        IDictionary<K, V> dict,
        IList<K> keys,
        bool keepElementInKeys = true
    )
    {
        if (keepElementInKeys)
        {
            return dict.Where(kvp => keys.Contains(kvp.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        return dict.Where(kvp => !keys.Contains(kvp.Key))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public static IDictionary<K, V> FilterDictionary<K, V>(
        IDictionary<K, V> dict,
        ISet<K> keys,
        bool keepElementInKeys = true
    )
    {
        if (keepElementInKeys)
        {
            return dict.Where(kvp => keys.Contains(kvp.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        return dict.Where(kvp => !keys.Contains(kvp.Key))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public static ISet<K> MapDictionaryKeyToSet<K, V>(IDictionary<K, V> dict)
    {
        ISet<K> set = new HashSet<K>();

        foreach (K key in dict.Keys)
        {
            set.Add(key);
        }

        return set;
    }

    public static Sprite LoadSprite(string path, string subName = "")
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);

        if (subName.Trim() == "")
        {
            return sprites[0];
        }

        foreach (var sprite in sprites)
        {
            if (sprite.name == subName)
            {
                return sprite;
            }
        }
        return null;
    }

    public static float GetNormalizedAngle(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Atan2(pos1.y - pos2.y, pos1.x - pos2.x) * Mathf.Rad2Deg + 180f;
    }

    public static string GetTimeString(float time)
    {
        int seconds = Mathf.RoundToInt(time);
        int minutes = seconds / 60;
        seconds %= 60;

        string timerStr = minutes.ToString() + ":";

        if (seconds < 10)
        {
            timerStr += "0";
        }

        timerStr += seconds.ToString();

        return timerStr;
    }
}
