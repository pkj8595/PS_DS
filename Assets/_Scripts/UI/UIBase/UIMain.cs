using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIBase
{
    [field : SerializeField] public UI_MoveResource UIMoveResource { get; set; }

   
    public override void Init(UIData uiData)
    {
        base.Init(uiData);
        
    }
    
    public override void UpdateUI()
    {
        base.UpdateUI();
       
    }

    public override void Close()
    {
        base.Close();
    }



}
