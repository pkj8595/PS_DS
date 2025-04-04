using System;
using UnityEngine;

public class ProjectileComponent : MonoBehaviour
{
    private Action<Collider> onHit; // 충돌 시 실행할 Action

    public void Initialize(Action<Collider> onHitAction)
    {
        onHit = onHitAction;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌 시 전달된 Action 실행
        onHit?.Invoke(other);

    }
}
