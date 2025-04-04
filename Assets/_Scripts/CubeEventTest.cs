using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEventTest : MonoBehaviour
{
    public Define.EGoodsType goods;

    private void Awake()
    {
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Test2();
        }

    }
    


    public void Test2()
    {
        var uimain = Managers.UI.GetUI<UIMain>() as UIMain;
        uimain.UIMoveResource.QueueAddItem(transform.position, goods, 100);
    }

    
   
}
