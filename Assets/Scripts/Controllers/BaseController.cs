using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Vector3 _moveDestPos;

    [SerializeField]
    protected GameObject _lockTarget;

    [SerializeField]
    protected Define.State _state = Define.State.Idle;

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:
                    //anim.SetBool("attack", false);
                    break;

                case Define.State.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    //anim.SetFloat("speed", _stat.MoveSpeed);
                    //anim.SetBool("attack", false);
                    break;

                case Define.State.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    //anim.SetFloat("speed", 0);
                    //anim.SetBool("attack", false);
                    break;

                case Define.State.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0); // 반복 재생
                    //anim.SetBool("attack", true);
                    break;

            }
        }
    }

    public abstract void Init();

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        switch (State)
        {
            case Define.State.Die:
                UpdateDie(); 
                break;

            case Define.State.Moving:
                UpdateMoving(); 
                break;

            case Define.State.Idle:
                UpdateIdle(); 
                break;

            case Define.State.Skill:
                UpdateSkill(); 
                break;

        }
    }

    protected virtual void UpdateDie() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }
}
