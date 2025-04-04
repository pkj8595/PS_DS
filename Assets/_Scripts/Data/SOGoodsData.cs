using UnityEngine;

[CreateAssetMenu(fileName = "SOGoodsData", menuName = "Data/GoodsData", order = 1)]
public class SOGoodsData : SOData
{
    public enum EGoodsType
    {
        mineral,
        gas,
    }
    public Sprite sprite;
    public EGoodsType goodsType;
    public string title;
    public string desc;
}
