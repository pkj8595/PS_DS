using Cysharp.Threading.Tasks;
using GameKit.Dependencies.Utilities.ObjectPooling.Examples;
using UnityEngine;

[CreateAssetMenu(fileName = "ForwardProjectile", menuName = "Data/Skill/Affect/ForwardProjectile", order = 1)]
public class SOSkillEffect_ForwardProjectile : SkillEffectBase
{
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float lifeTime = 5f;
    public float damageAmount;

    public override async UniTask ApplyEffect(IDamageable caster, IDamageable target)
    {
        GameObject projectile = GameObject.Instantiate(projectilePrefab, caster.Transform.position, Quaternion.identity);
        var projectileComponent = projectile.GetComponent<ProjectileComponent>();

        // 충돌 시 실행할 효과 정의
        projectileComponent.Initialize(collider =>
        {
            IDamageable hitUnit = collider.GetComponent<IDamageable>();
            if (hitUnit != null)
            {
              //todo
            }
        });



        // 생존 시간 후 투사체 제거
        await UniTask.Delay((int)(lifeTime * 1000));
        if (projectile != null) GameObject.Destroy(projectile);
    }
}
