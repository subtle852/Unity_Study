using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected int _attack;
    [SerializeField]
    protected int _defense;
    [SerializeField]
    protected float _moveSpeed;
    [SerializeField]
    protected float _rotateSpeed;

    public int Level { get { return _level; } set { _level = value; } }
    public int HP { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    public float RotateSpeed { get { return _rotateSpeed; } set { _rotateSpeed = value; } }

    private void Start()
    {
        _level = 1;
        _hp = 100;
        _maxHp = 100;
        _attack = 10;
        _defense = 5;
        _moveSpeed = 5.0f;
        _rotateSpeed = 10.0f;
    }

    public virtual void OnAttacked(Stat attackerStat)
    {
        int damage = Mathf.Max(0, attackerStat.Attack - Defense);
        HP -= damage;
        if (HP < 0)
        { 
            HP = 0;
            OnDead(attackerStat);
        }

    }

    protected virtual void OnDead(Stat attackerStat)
    {
        PlayerStat playerStat = attackerStat as PlayerStat;
        if (playerStat != null)
        {
            playerStat.Exp += 15;
        }

        Managers.Game.Despawn(gameObject);
    }

}
