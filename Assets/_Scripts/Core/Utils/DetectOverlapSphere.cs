using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectOverlapSphere : MonoBehaviour, IDetectComponent
{
    [Header("OverlapSphere Settings")]
    public float sphereRadius = 1.0f;             // Sphere의 반경
    public Vector3 sphereCenterOffset;            // Sphere의 중심 위치 오프셋
    public LayerMask collisionLayers;             // 충돌 레이어 설정

    public Collider[] DetectCollision()
    {
        Vector3 sphereCenter = transform.position + sphereCenterOffset;
        Collider[] hitColliders = Physics.OverlapSphere(sphereCenter, sphereRadius, collisionLayers);

        return hitColliders;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + sphereCenterOffset, sphereRadius);
    }
}
