using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    int _exp;
    [SerializeField]
    int _gold;

    public int Exp 
    { 
        get { return _exp; } 
        set 
        { 
            _exp = value;

            int tmpLevel = Level;
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDict.TryGetValue(tmpLevel + 1, out stat) == false) // 다음 레벨 없는 경우
                    break;
                if (_exp < stat.totalExp)
                    break;

                tmpLevel++;
            }

            if (Level != tmpLevel)
            {
                Level = tmpLevel;
                SetStat(Level);
                Debug.Log($"Level Up! CurrentLevel is: {Level}");
            }
        } 
    }

    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        _level = 1;
        _exp = 0;
        SetStat(1);

        _gold = 0;
        _moveSpeed = 5.0f;
        _rotateSpeed = 10.0f;

    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[level];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
        _defense = stat.defense;
    }

    protected override void OnDead(Stat attackerStat)
    {
        Debug.Log("Player Dead");
    }

}
