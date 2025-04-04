using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Text _txtTitle;
    [SerializeField] private Text _txtContent;
    public void ShowToolTip(Vector2 position, string title, string desc)
    {
        UpdateTooltipPosition(position);
        _txtTitle.text = title;
        _txtContent.text = desc;
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    private void UpdateTooltipPosition(Vector2 position)
    {
        Vector2 mousePos = Input.mousePosition;

        // 화면 크기 가져오기
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 screenCenter = screenSize / 2;
        Vector2 newAnchor = Vector2.zero;
        Vector2 newPivot = Vector2.zero;

        // 마우스가 화면의 좌우 어디에 가까운지 판단
        if (mousePos.x > screenCenter.x)
        {
            newAnchor.x = 1; // 오른쪽
            newPivot.x = 1;
        }
        else
        {
            newAnchor.x = 0; // 왼쪽
            newPivot.x = 0;
        }

        // 마우스가 화면의 상하 어디에 가까운지 판단
        if (mousePos.y > screenCenter.y)
        {
            newAnchor.y = 1; // 위쪽
            newPivot.y = 1;
        }
        else
        {
            newAnchor.y = 0; // 아래쪽
            newPivot.y = 0;
        }

        _rect.anchorMin = newAnchor;
        _rect.anchorMax = newAnchor;
        _rect.pivot = newPivot;

        //_rect.anchoredPosition = mousePos;
        _rect.position = position;
    }
}
