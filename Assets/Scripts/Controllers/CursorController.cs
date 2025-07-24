using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    Texture2D _attackIcon;
    Texture2D _handIcon;
    Vector2 _attackIconPos;
    Vector2 _handIconPos;

    enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    CursorType _cursorType = CursorType.None;

    int _mouseClickMask_GroundOrMonster = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
        _attackIconPos = new Vector2(_attackIcon.width / 5, 0);
        _handIconPos = new Vector2(_handIcon.width / 3, 0);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _mouseClickMask_GroundOrMonster))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if (_cursorType == CursorType.Attack)
                    return;

                Cursor.SetCursor(_attackIcon, _attackIconPos, CursorMode.Auto);
                _cursorType = CursorType.Attack;
            }
            else if (hit.collider.gameObject.layer == (int)Define.Layer.Ground)
            {
                if (_cursorType == CursorType.Hand)
                    return;

                Cursor.SetCursor(_handIcon, _handIconPos, CursorMode.Auto);
                _cursorType = CursorType.Hand;
            }
        }
    }
}
