using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    PlayerStat _stat;
    
    int _mouseClickMask_GroundOrMonster = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    bool _stopSkill = false;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;

        _stat = gameObject.GetOrAddComponent<PlayerStat>();

        //Managers.Input.KeyAction -= OnKeyboardEvent;
        //Managers.Input.KeyAction += OnKeyboardEvent;
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);

        // TEMP
        //UI_Button ui = Managers.UI.ShowPopupUI<UI_Button>();
        //Managers.UI.ClosePopupUI(ui);
        //Managers.UI.ShowSceneUI<UI_Inven>();
    }

    protected override void UpdateDie()
    {

    }

    protected override void UpdateMoving()
    {
        if (_lockTarget != null)
        {
            _moveDestPos = _lockTarget.transform.position; 
            float distance = (_moveDestPos - transform.position).magnitude;
            if (distance <= 1.0f)
            {
                State = Define.State.Skill;
                return;
            }
        }

        Vector3 dir = _moveDestPos - transform.position;
        dir.y = 0.0f;
        if (dir.magnitude < 0.01f)
        {
            State = Define.State.Idle;
        }
        else
        {
            //NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            //float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            //nma.Move(dir.normalized * moveDist);
            
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0))
                    return;

                State = Define.State.Idle;
                return;
            }

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    protected override void UpdateIdle()
    {

    }

    protected override void UpdateSkill()
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
            targetStat.OnAttacked(_stat);
        }

        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
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

    void OnMouseEvent(Define.MouseEvent evt)
    {   
        switch (State)
        {
            case Define.State.Die:
                
                break;

            case Define.State.Moving:
                OnMouseEvent_IdleOrMoving(evt);
                break;

            case Define.State.Idle:
                OnMouseEvent_IdleOrMoving(evt);
                break;

            case Define.State.Skill:
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

                    _moveDestPos = hit.point;
                    State = Define.State.Moving;
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
                            _moveDestPos = hit.point;
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
