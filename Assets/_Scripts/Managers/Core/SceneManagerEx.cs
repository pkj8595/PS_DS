using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : ManagerBase
{
    private BaseScene _currentScene;

    public override void Init()
    {
        base.Init();
        _currentScene = GameObject.FindFirstObjectByType<BaseScene>();
    }

    public override void Clear()
    {
        CurrentScene.Clear();
    }

    public BaseScene CurrentScene
    {
        get 
        {
            if (SceneManager.GetActiveScene().name != _currentScene.GetType().Name)
                _currentScene = GameObject.FindFirstObjectByType<BaseScene>();

            return _currentScene; 
        }
    }

    public void LoadScene(Define.Scene type)
    {
        
    }

}
