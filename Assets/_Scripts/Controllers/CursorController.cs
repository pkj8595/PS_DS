using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    int _mask = Define.Layer.Ground.ToInt();
    Texture2D _attackIcon;
    Texture2D _handIcon;
    CursorType _cursorType = CursorType.None;

    public enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Sprite/testImg");
        _handIcon = Managers.Resource.Load<Texture2D>("Sprite/testImg");
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, _mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Pawn)
            {
                if (_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }
            }
            else
            {
                if (_cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }
}
