using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public int level;
    public int hp;
    public int attack;
}

[Serializable]
public class StatData
{
    public List<Stat> stats = new List<Stat>();
}

public class DataManager
{
    public Dictionary<int, Stat> StatDict = new Dictionary<int, Stat>();

    public void Init()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/StatData");
        StatData data = JsonUtility.FromJson<StatData>(textAsset.text);

        foreach (Stat stat in data.stats )
            StatDict.Add(stat.level, stat);


    }
}
