using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase 
{
    private Transform _uiRoot;
    //List지만 stack처럼 활용한다.
    private LinkedList<UIBase> _uiStack = new ();

    public int baseSortingOrder = 0;
    public int sortingOrderAddValue = 100;

    UIStateBarGroup uiStateBarGroup;

    public void Init(GameObject root)
    {
        base.Init();
        if (_uiRoot == null)
        {
            var obj = root.transform.Find("@UIManager");
            if (obj == null)
            {
                _uiRoot = new GameObject("@UIManager").transform;
                _uiRoot.parent = root.transform;
            }
            else
            {
                _uiRoot = obj.transform;
            }

        }
    }

    /// <summary>
    /// 팝업 생성 및 캐싱되어 있으면 반환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public UIBase ShowUI<T>(UIData uiData = null) where T : UIBase
    {
        //캐싱된 UI 찾기
        UIBase ui = GetUI<T>();
       
        if (ui == null)
        {
            ui = Managers.Resource.LoadUI<T>(_uiRoot);
            if (ui == null)
                return null;
            _uiStack.AddLast(ui);
        }
        else
        {
            _uiStack.Remove(ui);
            _uiStack.AddLast(ui);
        }

        ui.SetUIBaseData();
        ui.Init(uiData);
        UpdateSortingOrder();
        ui.UpdateUI();
        return ui;
    }

    public UIBase ShowUIPopup<T>(UIData uiData = null) where T : UIPopup
    {
        //캐싱된 UI 찾기
        UIBase ui = GetUI<T>();

        if (ui == null)
        {
            ui = Managers.Resource.LoadUIPopup<T>(_uiRoot);
            if (ui == null)
                return null;
            _uiStack.AddLast(ui);
        }
        else
        {
            _uiStack.Remove(ui);
            _uiStack.AddLast(ui);
        }
        ui.SetUIBaseData();
        ui.Init(uiData);
        UpdateSortingOrder();
        ui.UpdateUI();
        return ui;
    }

    /// <summary>
    /// 현재 켜져있는 UI의 sortingorder 갱신
    /// </summary>
    public void UpdateSortingOrder()
    {
        int activeCount = 0;

        foreach(var ui in _uiStack)
        {
            if (ui.isActiveAndEnabled)
            {
                activeCount++;
                ui.SetSortingOrder(baseSortingOrder + (activeCount * sortingOrderAddValue));
            }
        }

    }

    public void CloseUI<T>() where T : UIBase
    {
        UIBase targetUI = GetUI<T>();
        targetUI.Close();
    }

    public UIBase GetUI(string uiName)
    {
        foreach (var ui in _uiStack)
        {
            if (ui.UIName == uiName)
                return ui;
        }

        return null;
    }

    public UIBase GetUI<T>() where T : UIBase
    {
        foreach (var ui in _uiStack)
        {
            if (ui is T)
            {
                return ui;
            }
        }

        return null;
    }

    public UIBase GetTopUI()
    {
        if (0 < _uiStack.Count)
            return _uiStack.Last.Value;
        else
            return null;
    }


    public void ShowToastMessage(string message)
    {
        var toast = ShowUIPopup<UIToastMessage>() as UIToastMessage;
        toast.SetMessage(message);
    }

    public override void Clear()
    {
        base.Clear();

        GameObject.Destroy(_uiRoot);
        _uiStack.Clear();
    }

    public void ShowPawnDialog(Transform pawn, string str)
    {
        if (uiStateBarGroup == null)
            uiStateBarGroup = Managers.UI.ShowUI<UIStateBarGroup>() as UIStateBarGroup;

        uiStateBarGroup.ShowDialog(pawn, str);
    }
}
