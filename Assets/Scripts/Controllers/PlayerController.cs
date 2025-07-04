using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _positionSpeed = 10.0f;
    [SerializeField]
    float _rotationSpeed = 10.0f;

    bool _mouseMoveToDest = false;
    Vector3 _mouseMoveDestPos;

    float wait_run_ratio = 0;

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    void Update()
    {
        if (_mouseMoveToDest == true)
        {
            Vector3 dir = _mouseMoveDestPos - transform.position;
            dir.y = 0.0f;
            if(dir.magnitude < 0.0001f)
            {
                _mouseMoveToDest = false;
            }
            else
            {
                float moveDist = Mathf.Clamp(_positionSpeed * Time.deltaTime, 0, dir.magnitude);

                transform.position += dir.normalized * moveDist;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            }
        }

        if(_mouseMoveToDest == true)
        {
            wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
            Animator animator = GetComponent<Animator>();
            animator.SetFloat("wait_run_ratio", wait_run_ratio);
            animator.Play("WAIT_RUN");
        }
        else
        {
            wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
            Animator animator = GetComponent<Animator>();
            animator.SetFloat("wait_run_ratio", wait_run_ratio);
            animator.Play("WAIT_RUN");
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
        _mouseMoveToDest = false;

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

        Debug.Log("OnMouseClicked");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            _mouseMoveDestPos = hit.point;
            _mouseMoveToDest = true;
        }
    }
}
