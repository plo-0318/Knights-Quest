using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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

    public static Vector2 GetDirectionFromAngle(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
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

    public static Vector3[] GeneratePosOffsets(int numPos, float radius)
    {
        float degreeBetweenPos = 360f / numPos;
        float currentDeg = 0;

        List<Vector3> pos = new List<Vector3>();

        for (int i = 0; i < numPos; i++)
        {
            Vector2 posOffset = new Vector2(
                Mathf.Cos(currentDeg * Mathf.Deg2Rad) * radius,
                Mathf.Sin(currentDeg * Mathf.Deg2Rad) * radius
            );

            pos.Add(posOffset);

            currentDeg += degreeBetweenPos;
        }

        return pos.ToArray();
    }

    public static void ShuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random();

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    public static Vector3[] GeneratePositionsAround(Vector3 pos, int numPos, float radius)
    {
        List<Vector3> newPos = new List<Vector3>();

        float angleOffset = 360f / numPos;
        float currentAngle = 0;

        for (int i = 0; i < numPos; i++)
        {
            Vector3 offset = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius,
                Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius
            );

            newPos.Add(pos + offset);

            currentAngle += angleOffset;
        }

        return newPos.ToArray();
    }

    public static Vector3 GenerateRandomPositionAround(
        Vector3 pos,
        float minDistance,
        float maxDistance
    )
    {
        return GenerateRandomPositionsAround(pos, 1, minDistance, maxDistance)[0];
    }

    public static Vector3[] GenerateRandomPositionsAround(
        Vector3 pos,
        int numPos,
        float minDistance,
        float maxDistance
    )
    {
        List<Vector3> randomPos = new List<Vector3>();

        for (int i = 0; i < numPos; i++)
        {
            float radius = UnityEngine.Random.Range(minDistance, maxDistance);
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * radius;

            randomPos.Add(new Vector3(pos.x + randomOffset.x, pos.y + randomOffset.y, pos.z));
        }

        return randomPos.ToArray();
    }
}
