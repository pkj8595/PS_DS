using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

public class UI_PawnDialog : MonoBehaviour
{
    public Text _txtDialog;
    public bool IsProcessing {get;set;}
    private Action _deActiveAction;

    Coroutine coDialog;

    public void ShowDialog(string str, Action DeActiveAction)
    {
        if (coDialog != null)
        {
            StopCoroutine(coDialog);
            coDialog = null;
        }

        _deActiveAction = DeActiveAction;
        coDialog = StartCoroutine(ShowTask(str));
    }

    IEnumerator ShowTask(string str)
    {
        IsProcessing = true;
        _txtDialog.gameObject.SetActive(true);

        int index = 0;
        while (index < str.Length)
        {
            index++;
            _txtDialog.text = str.Substring(0, index);
            yield return YieldCache.WaitForSeconds(0.1f);
        }

        yield return YieldCache.WaitForSeconds(1.0f);

        _txtDialog.text = string.Empty;
        _txtDialog.gameObject.SetActive(false);
        IsProcessing = false;
        _deActiveAction?.Invoke();
    }
}
