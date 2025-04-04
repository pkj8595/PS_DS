using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateBarGroup : UIBase
{
    //state bar
    public Dictionary<IDamageable, UI_HPbar> _dicUnit = new(); // 체력바를 가진 오브젝트들
    public Queue<UI_HPbar> _stateBarPool = new(); // hp바 풀링
    public UI_HPbar _stateBarPrefab; //hp 프리팹

    //dialog
    public Queue<UI_PawnDialog> _dialogPool = new(); // hp바 풀링
    public UI_PawnDialog _pawnDialogPrefab; //dialog prefab
    public Dictionary<Transform, UI_PawnDialog> _dicDialog = new();

    //parent
    public Transform dialogParent;
    public Transform stateParent;

    public override void Init(UIData uiData)
    {
        base.Init(uiData);
        _stateBarPrefab.gameObject.SetActive(false);
        _pawnDialogPrefab.gameObject.SetActive(false);
    }
    public override void SetSortingOrder(int sortingOrder)
    {
        canvas.sortingOrder = 1;
    }

    private void LateUpdate()
    {
        foreach (var unit in _dicUnit)
        {
            if (unit.Key == null || unit.Key.IsDead)
            {
                unit.Value.gameObject.SetActive(false);
                continue;
            }

            // 화면에서 보이는지 여부 확인
            Vector3 worldPosition = unit.Key.Transform.position + unit.Key.StateBarOffset;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            //화면 노출 여부
            if (screenPosition.z > 0 &&
                screenPosition.x > 0 && screenPosition.x < Screen.width &&
                screenPosition.y > 0 && screenPosition.y < Screen.height)
            {
                //화면에 노출 된다면
                unit.Value.gameObject.SetActive(true);
                unit.Value.OnUpdatePosition(screenPosition);
            }
            else
            {
                unit.Value.gameObject.SetActive(false);
            }
        }

        foreach(var pawnDialog in _dicDialog)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(pawnDialog.Key.position + new Vector3(0,1.7f,0));
            pawnDialog.Value.transform.position = screenPosition;
        }

    }

    #region StateBar

    public void AddUnit(IDamageable unit)
    {
        if (_dicUnit.ContainsKey(unit))
            return;
        var stateBar = GetOrCreateStateBar();
        stateBar.Init(unit);
        _dicUnit.Add(unit, stateBar);
    }


    public void RemoveUnit(IDamageable unit)
    {
        if (!_dicUnit.ContainsKey(unit))
            return;
        DeActiveStatebar(_dicUnit[unit]);
        _dicUnit.Remove(unit);
    }

    public void SetActive(IDamageable unit, bool isActive)
    {
        if (_dicUnit.TryGetValue(unit, out UI_HPbar value))
        {
            value.gameObject.SetActive(isActive);
        }
    }

    private UI_HPbar GetOrCreateStateBar()
    {
        UI_HPbar ret;
        if (_stateBarPool.Count > 0)
            ret = _stateBarPool.Dequeue();
        else
            ret = Instantiate(_stateBarPrefab, stateParent);

        ret.gameObject.SetActive(true);
        return ret;
    }

    private void DeActiveStatebar(UI_HPbar stateBar)
    {
        if(stateBar != null)
        {
            stateBar.Clear();
            stateBar.gameObject.SetActive(false);
            _stateBarPool.Enqueue(stateBar);
        }
    }
    #endregion

    #region pawn dialog
    public void ShowDialog(Transform pawn, string str)
    {
        UI_PawnDialog pawnDialog;
        if (_dicDialog.ContainsKey(pawn))
        {
            pawnDialog = _dicDialog[pawn];
        }
        else
        {
            pawnDialog = GetOrCreateDialog();
            _dicDialog.Add(pawn, pawnDialog);
        }

        
        pawnDialog.ShowDialog(str, ()=> 
        {
            _dicDialog.Remove(pawn);
            PushPoolDialog(pawnDialog);
        });
    }

    private UI_PawnDialog GetOrCreateDialog()
    {
        UI_PawnDialog ret;
        if (_dialogPool.Count > 0)
            ret = _dialogPool.Dequeue();
        else
            ret = Instantiate(_pawnDialogPrefab, dialogParent);

        ret.gameObject.SetActive(true);
        return ret;
    }

    private void PushPoolDialog(UI_PawnDialog dialog)
    {
        if (dialog != null)
        {
            dialog.gameObject.SetActive(false);
            _dialogPool.Enqueue(dialog);
        }
    }
    #endregion

}
