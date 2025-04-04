using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : ManagerBase
{
    public T Load<T>(string path) where T : Object
    {
        //1. original를 이미 들고 있으면 바로 사용
        if (typeof(T) == typeof(GameObject))
        {
            string name = GetObejctName(path);

            //GameObject go = Managers.Pool.GetOriginal(name);
            GameObject go = PoolManager1.Instance.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public T LoadUI<T>(Transform parent = null) where T : UIBase
    {
        string path = Define.Path.UI + typeof(T).Name;
        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject gameObj = GameObject.Instantiate(prefab, parent);
        gameObj.name = prefab.name;
        return gameObj.GetComponent<T>();
    }

    public T LoadUIPopup<T>(Transform parent = null) where T : UIBase
    {
        string path = Define.Path.UIPopup + typeof(T).Name;
        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject gameObj = GameObject.Instantiate(prefab, parent);
        gameObj.name = prefab.name;
        return gameObj.GetComponent<T>();
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Faild to load prefab {path}");
            return null;
        }

        /*//2. 혹시 풀링된 애가 있을까?
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;*/

        if (original.GetComponent<Poolable>() != null)
            return PoolManager1.Instance.Pop(original).gameObject;


        GameObject go = Object.Instantiate(original, parent);
        //go.name = original.name;
        return go;
    }

    public GameObject Instantiate(GameObject prefab, Transform parent = null)
    {
        if (prefab.GetComponent<Poolable>() != null)
            //return Managers.Pool.Pop(prefab, parent).gameObject;
            return PoolManager1.Instance.Pop(prefab).gameObject;

        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        //pool이 필요한 오브젝트면 풀링에 저장
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            //Managers.Pool.Push(poolable);
            PoolManager1.Instance.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }

    private string GetObejctName(string path)
    {
        string name = path;
        int index = name.LastIndexOf("/");
        if (index >= 0)
            name = name.Substring(index + 1);
        return name;
    }
}
