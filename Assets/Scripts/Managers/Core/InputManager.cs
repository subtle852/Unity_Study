using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public event Action KeyAction = null;
    public event Action<Define.MouseEvent> MouseAction = null;
    bool _mousePressed = false;
    float _mousePressedTime = 0.0f;

    public void OnUpdate()
    {
        if(GameObject.FindObjectOfType(typeof(EventSystem)) != null
            && EventSystem.current.IsPointerOverGameObject())
            return;

        if (KeyAction != null && Input.anyKey)
            KeyAction.Invoke();


        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (_mousePressed == false)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _mousePressedTime = Time.time;
                }

                MouseAction.Invoke(Define.MouseEvent.Press);
                _mousePressed = true;
            }
            else
            {
                if (_mousePressed == true)
                {
                    if (Time.time < _mousePressedTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.Click);

                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }

                _mousePressed = false;
                _mousePressedTime = 0.0f;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
