using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindFirstObjectByType(typeof(T)) as T;
                if (m_instance == null)
                {
                    GameObject obj = new GameObject($"@{typeof(T).Name}");
                    m_instance = obj.GetOrAddComponent<T>();
                }
                else
                    m_instance.name = $"@{typeof(T).Name}";
            }
            return m_instance;
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            m_instance = this as T;
        }
    }

    private void OnApplicationQuit()
    {
        m_instance = null;
    }
}
