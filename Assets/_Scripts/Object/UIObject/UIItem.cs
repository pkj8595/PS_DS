using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    public Image _imgItem;
    public Text _txtAmount;

    public void Init(int itemNum, int amount)
    {
        if(itemNum == 0 || amount == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        
        /*var item = ItemBase.GetItem(itemNum);
        _imgItem.sprite = Managers.Resource.Load<Sprite>($"Sprites/UI/Icon/{item.ImgStr}");
        _txtAmount.text = amount.ToString();*/
        gameObject.SetActive(true);
    }

    public void Init(int itemNum, int amount, bool isSpend)
    {
        if (itemNum == 0 || amount == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        /*var item = ItemBase.GetItem(itemNum);
        _imgItem.sprite = Managers.Resource.Load<Sprite>($"Sprites/UI/Icon/{item.ImgStr}");
        if (!isSpend)
            _txtAmount.text = $"<color=red>{amount}</color>";
        else
            _txtAmount.text = amount.ToString();*/
        gameObject.SetActive(true);
    }

}
