using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectManager : ManagerBase
{
    List<EffectAniController> _aniEffectList = new List<EffectAniController>(10);

    public override void Init()
    {
        base.Init();

    }
    public override void Clear() 
    {
        _aniEffectList.Clear();
    }


    /// <summary>
    /// ParticleSystem 플레이 만약 지속 데미지라면 parent 넣어주기
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pos"></param>
    /// <param name="normal"></param>
    /// <param name="parent"></param>
    /// <param name="effectType"></param>
    public ParticleSystem PlayEffect(string name, Vector3 pos, Vector3 normal, Transform parent = null)
    {
        var effect = LoadEffect(name,pos,normal,parent);
        effect.Play();
        return effect;
    }

    public ParticleSystem LoadEffect(string name, Vector3 pos, Vector3 normal, Transform parent)
    {
        string path = $"Prefebs/Effects/ParticleEffects/{name}";
        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject gameObj;
        if (parent == null)
            gameObj = GameObject.Instantiate(prefab, pos, Quaternion.LookRotation(normal));
        else
            gameObj = GameObject.Instantiate(prefab, pos, Quaternion.LookRotation(normal), parent);

        gameObj.name = prefab.name;
        return gameObj.GetComponent<ParticleSystem>();
    }

    public EffectAniController PlayAniEffect(string name, Transform parent = null, int sortingOrder = 200)
    {
        EffectAniController ret = GetPoolAniEffect();
        ret.name = name;
        ret.PlayAniEffect(name, parent, sortingOrder);
        return ret;
    }

    public EffectAniController PlayAniEffect(string name, Vector3 position, int sortingOrder = 200)
    {
        EffectAniController ret = GetPoolAniEffect();
        ret.name = name;
        ret.PlayAniEffect(name, position, sortingOrder);
        return ret;
    }

    private EffectAniController GetPoolAniEffect()
    {
        string path = $"Prefabs/Effects/EffectAnimation";
        GameObject prefab = Resources.Load<GameObject>(path);
        EffectAniController ret = GameObject.Instantiate(prefab).GetComponent<EffectAniController>();
        _aniEffectList.Add(ret);
        return ret;
    }
    public void RemoveAniEffect(EffectAniController controller)
    {
        controller.StopEffect();
    }
}
