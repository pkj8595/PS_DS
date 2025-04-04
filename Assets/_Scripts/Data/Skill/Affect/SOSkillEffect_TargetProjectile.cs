using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetProjectile", menuName = "Data/Skill/Affect/TargetProjectile", order = 1)]
public class SOSkillEffect_TargetProjectile : SkillEffectBase
{
    GameObject projectilePrafab;
    public float speed;

    public int damagePer;
    public override async UniTask ApplyEffect(IDamageable caster, IDamageable target)
    {
        GameObject projectile = Instantiate(projectilePrafab, 
                                            caster.ProjectileTF.position, 
                                            caster.ProjectileTF.rotation);

        await MoveProjectile(projectile, target);
        Destroy(projectile);
    }

    private async UniTask MoveProjectile(GameObject projectile, IDamageable target)
    {
        while (projectile != null && target != null)
        {
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position,
                                                                target.                                                                Transform.position,
                                                                speed * Time.deltaTime);

            if (Vector3.Distance(projectile.transform.position, target.Transform.position) < 0.1f)
                break;

            await UniTask.Yield();
        }
    }
}
