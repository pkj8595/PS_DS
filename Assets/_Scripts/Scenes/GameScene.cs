using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SCENE_TYPE = Define.Scene.GameScene;
        
        StartGame().Forget();
    }

    public override void Clear()
    {
        
    }


    async UniTaskVoid StartGame()
    {
        Managers.UI.ShowUI<UIMain>();
        await UniTask.NextFrame();

    }
}
