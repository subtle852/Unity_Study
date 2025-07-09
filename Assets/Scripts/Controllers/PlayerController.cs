using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _positionSpeed = 10.0f;
    [SerializeField]
    float _rotationSpeed = 10.0f;

    //bool _mouseMoveToDest = false; // state로 관리하기에 더이상 불필요
    Vector3 _mouseMoveDestPos;

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
    }

    PlayerState _state = PlayerState.Idle;

    void Start()
    {
        // 마우스 이동만 가능하도록 수정
        //Managers.Input.KeyAction -= OnKeyboard;
        //Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;

        Managers.Resource.Instantiate("UI/UI_Button");
    }

    void UpdateDie()
    {

    }

    void UpdateMoving()
    {
        Vector3 dir = _mouseMoveDestPos - transform.position;
        dir.y = 0.0f;
        if (dir.magnitude < 0.0001f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            float moveDist = Mathf.Clamp(_positionSpeed * Time.deltaTime, 0, dir.magnitude);

            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        // 애니메이션
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("speed", _positionSpeed);
    }

    void UpdateIdle()
    {
        // 애니메이션
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("speed", 0);
    }

    void Update()
    {
        switch(_state)
        {
            case PlayerState.Die:
                UpdateDie(); break;

            case PlayerState.Moving:
                UpdateMoving(); break;

            case PlayerState.Idle:
                UpdateIdle(); break;

        }
    }

    void OnKeyboard()
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
                Mathf.Clamp01(_rotationSpeed * Time.deltaTime));

            transform.position += Vector3.forward * Time.deltaTime * _positionSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(Vector3.back),
                Mathf.Clamp01(_rotationSpeed * Time.deltaTime));

            transform.position += Vector3.back * Time.deltaTime * _positionSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(Vector3.left),
                Mathf.Clamp01(_rotationSpeed * Time.deltaTime));

            transform.position += Vector3.left * Time.deltaTime * _positionSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(Vector3.right),
                Mathf.Clamp01(_rotationSpeed * Time.deltaTime));

            transform.position += Vector3.right * Time.deltaTime * _positionSpeed;
        }
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        // 마우스 누른 상태에서도 움직이도록 수정
        //if (evt != Define.MouseEvent.Click)
        //    return;

        if (_state == PlayerState.Die)
            return;

        Debug.Log("OnMouseClicked");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            _mouseMoveDestPos = hit.point;
            _state = PlayerState.Moving;
        }
    }
}
