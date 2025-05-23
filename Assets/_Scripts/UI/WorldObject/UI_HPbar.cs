using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_HPbar : MonoBehaviour
{
    [SerializeField] private Image imgHp;
    [SerializeField] private Image imgMp;
    [SerializeField] private Image imgHpSecond;
    [SerializeField] private Image imgMpSecond;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Text txtHp;
    public IDamageable Unit { get; private set; }
    float _hpSecondPercent = 0f;
    float _mpSecondPercent = 0f;
    Define.ETeam ETeam = Define.ETeam.Player1;

    public void Init(IDamageable unit)
    {
        Unit = unit;
        ShowHpColor(unit);
        _hpSecondPercent = 0f;
        _mpSecondPercent = 0f;
    }

    private void ShowHpColor(IDamageable unit)
    {
        if (unit.Team == Define.ETeam.Player1)
        {
            imgHp.color = Utils.HexToColor("#44fe90");//초록
            imgHpSecond.color = Utils.HexToColor("#aa2406");//어두운 빨강
        }
        else
        {
            imgHp.color = Utils.HexToColor("#c14a3b");//빨강
            imgHpSecond.color = Utils.HexToColor("#dad14b");//노랑
        }
        ETeam = unit.Team;
    }

    public void OnUpdatePosition(Vector3 position)
    {
        ShowHpColor(Unit);
        rectTransform.position = position;

        float hpPercent = Utils.Percent(Unit.Stat.Hp, Unit.Stat.MaxHp);
        float mpPercent = Utils.Percent(Unit.Stat.Mana, Unit.Stat.MaxMana);
        imgHp.rectTransform.localScale = new Vector2(hpPercent, 1f);
        imgMp.rectTransform.localScale = new Vector2(mpPercent, 1f);
        txtHp.text = ((int)Unit.Stat.Hp).ToString();

        UpdateSecondBar(imgHpSecond.rectTransform, ref _hpSecondPercent, hpPercent);
        UpdateSecondBar(imgMpSecond.rectTransform, ref _mpSecondPercent, mpPercent);

    }

    public void Clear()
    {
        Unit = null;
    }

    // 천천히 줄어드는 바의 스케일 업데이트
    private void UpdateSecondBar(RectTransform transform, ref float secondPercent, float targetPercent)
    {
        if (targetPercent < secondPercent)
        {
            secondPercent = Mathf.Lerp(secondPercent, targetPercent, Time.deltaTime * 3f);
            transform.localScale = new Vector3(secondPercent, 1, 1);
        }
        else
        {
            secondPercent = targetPercent;
            transform.localScale = new Vector3(secondPercent, 1,1);
        }
    }

}
