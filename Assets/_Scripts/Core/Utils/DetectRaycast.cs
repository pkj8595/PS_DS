using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRaycast : MonoBehaviour, IDetectComponent
{
    [Header("Raycast Settings")]
    public float rayDistance = 10.0f;            // Ray의 길이
    public Vector3 rayDirection = Vector3.forward; // Ray의 방향
    public LayerMask collisionLayers;            // 충돌 레이어 설정

    public Collider[] DetectCollision()
    {
        Collider[] colliders = new Collider[1];
        Vector3 rayOrigin = transform.position;
        Ray ray = new Ray(rayOrigin, transform.TransformDirection(rayDirection));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance, collisionLayers))
        {
            colliders[0] = hit.collider;
        }
        return colliders;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(rayDirection) * rayDistance);
    }
}
