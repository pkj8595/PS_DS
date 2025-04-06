using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

public class BuildingAniController : MonoBehaviour, ISkillMotion
{
    [SerializeField] private Animator _animator;

    public bool IsPlaying {get; private set; }


    void Start()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
    }
    private void OnDisable()
    {
        IsPlaying = false;
        
    }

    public void TrigerAnimation(Define.EBuildingAniState trigetAni)
    {
        switch (trigetAni)
        {
            case Define.EBuildingAniState.Idle:
                _animator.SetTrigger("Idle");
                break;
            case Define.EBuildingAniState.Ready:
                _animator.SetTrigger("Ready");
                break;
            case Define.EBuildingAniState.Dead:
                _animator.SetTrigger("Dead");
                IsPlaying = false;
                break;
            case Define.EBuildingAniState.Casting:
                _animator.SetTrigger("Casting");
                break;
        }
    }

    public async UniTask RunAnimation(ESkillMotionTriger triger, float delay)
    {
        IsPlaying = true;
        switch(triger)
        {
            case ESkillMotionTriger.Cast:
                _animator.SetTrigger("Cast");
                await UniTask.Delay((int)(delay * 1000), cancellationToken: destroyCancellationToken);
                break;
            case ESkillMotionTriger.MeleeAttack:
                _animator.SetTrigger("MeleeAttack");
                await UniTask.WaitUntil(() => IsPlaying is false, cancellationToken:destroyCancellationToken);
                break;
            case ESkillMotionTriger.RangedAttack:
                _animator.SetTrigger("RangedAttack");
                await UniTask.WaitUntil(() => IsPlaying is false, cancellationToken: destroyCancellationToken);
                break;
        }
    }

    /// <summary>
    /// Called by Animation Event
    /// </summary>
    public void OnAttackAniCallback()
    {
        IsPlaying = false;
    }
}
