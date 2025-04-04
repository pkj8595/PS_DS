using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBase
{
    public virtual void Init() 
    {
        
        //Debug.Log($"<color=green>Init : {GetType().Name} </color>");
    }
    public virtual void OnUpdate() { }
    public virtual void Clear() { }
}
