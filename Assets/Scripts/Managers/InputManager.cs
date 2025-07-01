using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public event Action KeyAction = null;
    public event Action<Define.MouseEvent> MouseAction = null;
    bool _mousePressed = false;

    public void OnUpdate()
    {
        if (KeyAction != null && Input.anyKey)
            KeyAction.Invoke();


        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                MouseAction.Invoke(Define.MouseEvent.Press);
                _mousePressed = true;
            }
            else
            {
                if (_mousePressed == true)
                    MouseAction.Invoke(Define.MouseEvent.Click);

                _mousePressed = false;
            }
        }
    }
}
