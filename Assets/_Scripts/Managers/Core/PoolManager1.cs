using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager1 : MonoSingleton<PoolManager1>
{
    readonly Dictionary<string, PoolContainer> _dicPoolContainer = new();

    public void Init()
    {

    }

    public void Push(Poolable poolable)
    {
        string originerName = poolable.OriginerName;
        if (_dicPoolContainer.ContainsKey(originerName) is false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _dicPoolContainer[originerName].Push(poolable);
    }

    public Poolable Pop(GameObject originer)
    {
        if (_dicPoolContainer.ContainsKey(originer.name))
        {
            return _dicPoolContainer[originer.name].Pop();
        }
        else
        {
            PoolContainer container = new PoolContainer(originer, 10, this.transform);
            _dicPoolContainer.Add(originer.name, container);
            return container.Pop();
        }
    }

    public void Clear()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform obj = transform.GetChild(i);
            Destroy(obj.gameObject);
        }
        _dicPoolContainer.Clear();
    }

    public GameObject GetOriginal(string name)
    {
        if (_dicPoolContainer.ContainsKey(name) is false)
            return null;

        return _dicPoolContainer[name].OriginObj;
    }


    private class PoolContainer
    {
        public Transform Root { get; set; }

        public GameObject OriginObj => _originObj;

        readonly Stack<Poolable> _stack;
        readonly GameObject _originObj;

        public PoolContainer(GameObject original, int initPoolCount, Transform parent)
        {
            _stack = new Stack<Poolable>(initPoolCount);
            Root = new GameObject(original.name).transform;
            _originObj = original;
            Root.SetParent(parent);

            for (int i = 0; i < initPoolCount; i++)
                Push(CloneObj());
        }

        public void Push(Poolable poolObj)
        {
            _stack.Push(poolObj);
            poolObj.gameObject.SetActive(false);
            poolObj.isUsing = false;
        }

        public Poolable Pop()
        {
            Poolable ret;
            if (_stack.Count > 0)
                ret = _stack.Pop();
            else
                ret = CloneObj();

            ret.isUsing = true;
            ret.gameObject.SetActive(true);
            return ret;
        }

        private Poolable CloneObj()
        {
            var clone = Object.Instantiate<GameObject>(_originObj, Root);
            var ret = clone.GetOrAddComponent<Poolable>();
            ret.OriginerName = _originObj.name;
            return ret;
        }

    }
}
