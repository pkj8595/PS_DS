using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SCENE_TYPE = Define.Scene.Login;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(Define.Scene.GameScene.ToString());
        }
    }

    public override void Clear()
    {
        Debug.Log("LoginScene Clear");
    }

}
