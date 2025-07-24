using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;

    Vector3 _mouseMoveDestPos;

    int _mouseClickMask_GroundOrMonster = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    GameObject _lockTarget;

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

    PlayerState _state = PlayerState.Idle;


    void Start()
    {
        _stat = gameObject.GetOrAddComponent<PlayerStat>();

        // 마우스 이동만 가능하도록 수정
        //Managers.Input.KeyAction -= OnKeyboardEvent;
        //Managers.Input.KeyAction += OnKeyboardEvent;
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

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
        Vector3 dir = _mouseMoveDestPos - transform.position;
        dir.y = 0.0f;
        if (dir.magnitude < 0.01f)
        {
            _state = PlayerState.Idle;
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

                _state = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        // 애니메이션
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("speed", _stat.MoveSpeed);
    }

    void UpdateIdle()
    {
        // 애니메이션
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("speed", 0);
    }

    void Update()
    {
        switch (_state)
        {
            case PlayerState.Die:
                UpdateDie(); break;

            case PlayerState.Moving:
                UpdateMoving(); break;

            case PlayerState.Idle:
                UpdateIdle(); break;

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
        if (_state == PlayerState.Die)
            return;

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
                    _state = PlayerState.Moving;

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
                    if (_lockTarget != null)
                    {
                        _mouseMoveDestPos = _lockTarget.transform.position;
                    }
                    else
                    {
                        if (raycastHit == true)
                            _mouseMoveDestPos = hit.point;
                    }
                }
                break;
            case Define.MouseEvent.PointerUp:
                {
                    _lockTarget = null;
                }
                break;

        }
    }
}
