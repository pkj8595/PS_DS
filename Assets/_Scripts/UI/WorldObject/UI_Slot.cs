using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UI_ToolTip _tooltip;
    public bool HasData = false;
    public void SetToolTip(UI_ToolTip tooltip)
    {
        _tooltip = tooltip;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(HasData)
            _tooltip.ShowToolTip(eventData.position, SetTitleStr(), SetDescStr());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(HasData)
            _tooltip.HideToolTip();
    }

    public virtual string SetTitleStr()
    {
        return string.Empty;
    }
    public virtual string SetDescStr()
    {
        return string.Empty;
    }
}
