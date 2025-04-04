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

    public async UniTask RunAnimation(ESkillMotionTriger triger, float delay)
    {
        IsPlaying = true;
        switch(triger)
        {
            case ESkillMotionTriger.Cast:
                _animator.SetTrigger("Cast");
                await UniTask.Delay((int)(delay * 1000));
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
