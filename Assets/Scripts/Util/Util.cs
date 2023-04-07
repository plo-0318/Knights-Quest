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
}
