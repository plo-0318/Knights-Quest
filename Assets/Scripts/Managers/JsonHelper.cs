using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

[System.Serializable]
public class SkillData
{
    public string displayName;
    public string name;
    public string description;
    public string lv2Effect;
    public string lv3Effect;
    public string lv4Effect;
    public string lv5Effect;
    public float damage;
    public float cooldown;

    public SkillData(SkillData other)
    {
        displayName = other.displayName;
        name = other.name;
        description = other.description;
        lv2Effect = other.lv2Effect;
        lv3Effect = other.lv3Effect;
        lv4Effect = other.lv4Effect;
        lv5Effect = other.lv5Effect;
        damage = other.damage;
        cooldown = other.cooldown;
    }
}
