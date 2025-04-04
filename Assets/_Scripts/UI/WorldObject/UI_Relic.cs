using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_Relic : MonoBehaviour
{
    [SerializeField] private Image _imgIcon;

    public void Init(string iconStr)
    {
        _imgIcon.sprite = Managers.Resource.Load<Sprite>(Define.Path.UIIcon + iconStr);

    }

}
