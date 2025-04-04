using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;


public class CameraController : MonoBehaviour
{
    [SerializeField] private float _cameraSpeed;
    [SerializeField] private GameObject _cameraTarget;
    [SerializeField] private Vector3 _offsetPositionFormTarget = new(-24f, 20f, -24f);
    [SerializeField] private Vector3 _offsetRorationFormTarget = new(30f, 45f, 0);

    [SerializeField] private float _rotateSpeed = 46f;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _mouseWheelScale = 0.01f;
    //public ProPixelizer.CameraSnapSRP cameraSnap;
    private Camera _camera;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }
    private void Start()
    {

        _cameraTarget.transform.ResetTransform();
        transform.SetPositionAndRotation(_offsetPositionFormTarget,
                                         Quaternion.Euler(_offsetRorationFormTarget));
        _cameraTarget.transform.Rotate(0f, 45f, 0f);

       
    }

    public void Update()
    {
        /*//마우스
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float clampSize = cameraSnap.PixelSize - scrollInput * _mouseWheelScale;
        cameraSnap.PixelSize = Mathf.Clamp(clampSize, 0.007f, 0.039f);*/
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float clampSize = _camera.orthographicSize - scrollInput * _mouseWheelScale;
        _camera.orthographicSize = Mathf.Clamp(clampSize, 3.0f, 14f);
    }

    public void MoveWASD()
    {
        //카메라 이동로직
        Vector3 moveValue = Vector3.zero;
        int rotateValue = 0;
        if (Input.GetKey(KeyCode.Q))
            rotateValue = 1;
        else if (Input.GetKey(KeyCode.E))
            rotateValue = -1;

        moveValue.z = Input.GetAxis("Vertical");
        moveValue.x = Input.GetAxis("Horizontal");

        Vector3 targetForward = _cameraTarget.transform.forward;
        Vector3 targetRight = _cameraTarget.transform.right;
        targetForward.y = 0;
        targetRight.y = 0;

        Vector3 moveDir = (targetForward.normalized * moveValue.z +
                            targetRight.normalized * moveValue.x).normalized;

        _cameraTarget.transform.position += moveDir * _moveSpeed * Time.deltaTime;
        _cameraTarget.transform.Rotate(0, rotateValue * _rotateSpeed * Time.deltaTime, 0);

    }

}
