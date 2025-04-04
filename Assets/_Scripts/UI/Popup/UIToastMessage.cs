using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class UIToastMessage : UIPopup
{
    public Text _txtMessage;
    public CanvasGroup _canvasGroup;
    Sequence sequence;
    Coroutine coroutine;

    public override void Init(UIData uiData)
    {
        base.Init(uiData);
    }

    private void OnEnable()
    {
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InQuad);
    }

    public override void SetSortingOrder(int sortingOrder)
    {
        canvas.sortingOrder = 10000;
    }

    private void FadeReset()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        if (sequence != null && sequence.IsActive() && sequence.IsPlaying())
        {
            sequence.Kill();
            sequence = null;
        }

        _canvasGroup.alpha = 1;
    }

    public void SetMessage(string message)
    {
        FadeReset();
        _txtMessage.text = message;
        gameObject.SetActive(true);
        coroutine = StartCoroutine(RunFade());
    }

    IEnumerator RunFade()
    {
        yield return YieldCache.WaitForSeconds(2f);
        Fade();
    }

    private void Fade()
    {
        sequence = DOTween.Sequence();
        sequence.Append(_canvasGroup.DOFade(0f, 0.3f));
        sequence.AppendCallback(() => { SetActive(false); });
    }


    
}
