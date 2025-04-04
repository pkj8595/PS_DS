using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectOverlapBox : MonoBehaviour, IDetectComponent
{
    [Header("OverlapBox Settings")]
    public Vector3 boxSize = new Vector3(1.0f, 1.0f, 1.0f); // Box의 크기
    public Vector3 boxCenterOffset;                         // Box의 중심 위치 오프셋
    public LayerMask collisionLayers;                       // 충돌 레이어 설정
    public Quaternion boxOrientation = Quaternion.identity; // Box의 회전 설정

    public Collider[] DetectCollision()
    {
        Vector3 boxCenter = transform.TransformPoint(boxCenterOffset);
        Quaternion finalOrientation = transform.rotation * boxOrientation;
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize / 2, finalOrientation, collisionLayers);
        return hitColliders;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.matrix = Matrix4x4.TRS(transform.position , transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(boxCenterOffset, boxSize); 
    }
}
