using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _positionSpeed = 10.0f;

    [SerializeField]
    private float _rotationSpeed = 10.0f;

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
    }

    void Update()
    {

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
}
