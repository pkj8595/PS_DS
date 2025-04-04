using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAniController: MonoBehaviour
{
    public SpriteRenderer[] renderers = new SpriteRenderer[3];
    public Animator _animator;
    public void Awake()
    {
        if(_animator == null)
            _animator = GetComponent<Animator>();
    }

    public void PlayAniEffect(string effectStr, Vector3 position, int sortingOrder = 200)
    {
        _animator.enabled = true;
        _animator.SetBool("IsRunning", true);
        foreach (SpriteRenderer render in renderers)
        {
            render.sortingOrder = sortingOrder;
        }

        transform.position = position;
        //transform.rotation = Camera.main.transform.rotation;
        _animator.Play(effectStr);
    }

    public void PlayAniEffect(string effectStr, Transform parent, int sortingOrder = 200)
    {
        transform.SetParent(parent);
        PlayAniEffect(effectStr, parent.position, sortingOrder);
    }

    public void StopEffect()
    {
        _animator.SetBool("IsRunning", false);
        _animator.enabled = false;
        gameObject.SetActive(false);
    }

    public bool IsRunning()
    {
        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return !stateInfo.IsName("Empty");
    }


}
