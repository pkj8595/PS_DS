using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SCENE_TYPE { get; protected set; } = Define.Scene.Unknown;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
       
    }

    public abstract void Clear();

}
