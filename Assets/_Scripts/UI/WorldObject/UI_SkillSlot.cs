using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSlot : UI_Slot
{
    [SerializeField] private Image _imgSlot;
    [SerializeField] private Image _imgCoolTime;
    [SerializeField] private Text _txtName;
    [SerializeField] private Text _txtCoolTime;
    Skill _skill;

    public void Init(Skill skill)
    {
        HasData = skill != null;
        _skill = skill;
        _imgSlot.sprite = Managers.Resource.Load<Sprite>($"{Define.Path.UIIcon}{skill.Icon}");
        _txtName.text = skill.Name;
        _imgCoolTime.fillAmount = 1.0f;
    }

    private void FixedUpdate()
    {
        if (this.gameObject.activeSelf && _skill != null)
        {
            float percent = _skill.GetCulcalatePercentCoolTime();
            _imgCoolTime.fillAmount = percent;
            _txtCoolTime.gameObject.SetActive(percent != 0f);
            _txtCoolTime.text = (_skill.CoolTime - (Time.time - _skill.LastRunTime)).ToString("0.0");
        }
    }

    public override string SetTitleStr()
    {
        return _skill.Name;
    }

    public override string SetDescStr()
    {
        SOSkillData data = _skill.data;
        System.Text.StringBuilder str = new();
        str.Append(_skill.Desc);
        str.Append("\n\n");
        str.Append($"desc : {data.desc}\n");
        str.Append($"require Mana : {data.requireMana}\n");
        str.Append($"coolTime :{data.coolTime}\n");

        return str.ToString();
    }
    
}
