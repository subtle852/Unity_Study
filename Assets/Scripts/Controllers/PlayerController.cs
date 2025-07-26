using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;

    Vector3 _mouseMoveDestPos;

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

    [SerializeField]
    PlayerState _state = PlayerState.Idle;

    public PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case PlayerState.Die:
                    //anim.SetBool("attack", false);
                    break;

                case PlayerState.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    //anim.SetFloat("speed", _stat.MoveSpeed);
                    //anim.SetBool("attack", false);
                    break;

                case PlayerState.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    //anim.SetFloat("speed", 0);
                    //anim.SetBool("attack", false);
                    break;

                case PlayerState.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0); // 반복 재생
                    //anim.SetBool("attack", true);
                    break;

            }
        }
    }

    int _mouseClickMask_GroundOrMonster = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    GameObject _lockTarget;

    void Start()
    {
        _stat = gameObject.GetOrAddComponent<PlayerStat>();

        // 마우스 이동만 가능하도록 수정
        //Managers.Input.KeyAction -= OnKeyboardEvent;
        //Managers.Input.KeyAction += OnKeyboardEvent;
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);

        // TEMP
        //UI_Button ui = Managers.UI.ShowPopupUI<UI_Button>();

        //Managers.UI.ClosePopupUI(ui);

        //Managers.UI.ShowSceneUI<UI_Inven>();
    }

    void UpdateDie()
    {

    }

    void UpdateMoving()
    {
        if (_lockTarget != null)
        {
            _mouseMoveDestPos = _lockTarget.transform.position; 
            float distance = (_mouseMoveDestPos - transform.position).magnitude;
            if (distance <= 1.0f)
            {
                State = PlayerState.Skill;
                return;
            }
        }

        Vector3 dir = _mouseMoveDestPos - transform.position;
        dir.y = 0.0f;
        if (dir.magnitude < 0.01f)
        {
            State = PlayerState.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            //transform.position += dir.normalized * moveDist;
            nma.Move(dir.normalized * moveDist);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0))
                    return;

                State = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    void UpdateIdle()
    {

    }

    void UpdateSkill()
    {
        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            Stat myStat = gameObject.GetComponent<Stat>();
            int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);
            Debug.Log(damage);
            targetStat.HP -= damage;
        }

        if (_stopSkill)
        {
            State = PlayerState.Idle;
        }
        else
        {
            State = PlayerState.Skill;
        }
    }

    void Update()
    {
        switch (State)
        {
            case PlayerState.Die:
                UpdateDie(); break;

            case PlayerState.Moving:
                UpdateMoving(); break;

            case PlayerState.Idle:
                UpdateIdle(); break;

            case PlayerState.Skill:
                UpdateSkill(); break;

        }
    }

    void OnKeyboardEvent()
    {
        // 이동 (다른 버전)
        //float horizontalInput = Input.GetAxisRaw("Horizontal");
        //float verticalInput = Input.GetAxisRaw("Vertical");

        //Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        //moveDirection = moveDirection.normalized;
        //transform.position += moveDirection * _positionSpeed * Time.deltaTime;

        // 여기서 회전을 추가하려면,
        // 굳이 Direction 추출 없이 로컬 좌표의 forward로만 움직이게 하고
        // 회전만 시키게 하면 된다


        // 이동 + 회전
        //_mouseMoveToDest = false;

        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(Vector3.forward),
                Mathf.Clamp01(_stat.RotateSpeed * Time.deltaTime));

            transform.position += Vector3.forward * Time.deltaTime * _stat.MoveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(Vector3.back),
                Mathf.Clamp01(_stat.RotateSpeed * Time.deltaTime));

            transform.position += Vector3.back * Time.deltaTime * _stat.MoveSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(Vector3.left),
                Mathf.Clamp01(_stat.RotateSpeed * Time.deltaTime));

            transform.position += Vector3.left * Time.deltaTime * _stat.MoveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(Vector3.right),
                Mathf.Clamp01(_stat.RotateSpeed * Time.deltaTime));

            transform.position += Vector3.right * Time.deltaTime * _stat.MoveSpeed;
        }
    }


    bool _stopSkill = false;
    void OnMouseEvent(Define.MouseEvent evt)
    {   
        switch (State)
        {
            case PlayerState.Die:
                
                break;

            case PlayerState.Moving:
                OnMouseEvent_IdleOrMoving(evt);
                break;

            case PlayerState.Idle:
                OnMouseEvent_IdleOrMoving(evt);
                break;

            case PlayerState.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                        _stopSkill = true;
                }
                break;

        }
    }

    void OnMouseEvent_IdleOrMoving(Define.MouseEvent evt)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        RaycastHit hit;
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mouseClickMask_GroundOrMonster);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit == false)
                        return;

                    _mouseMoveDestPos = hit.point;
                    State = PlayerState.Moving;
                    _stopSkill = false;

                    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                    {
                        _lockTarget = hit.collider.gameObject;
                    }
                    else if (hit.collider.gameObject.layer == (int)Define.Layer.Ground)
                    {
                        _lockTarget = null;
                    }

                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (raycastHit == true && _lockTarget == null)
                            _mouseMoveDestPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:
                {
                    _stopSkill = true;
                }
                break;

        }
    }
}
