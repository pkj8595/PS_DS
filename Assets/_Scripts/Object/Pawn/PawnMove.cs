using UnityEngine;

public class PawnMove : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _stopDistance;
    [SerializeField] private bool _isMove;
    [SerializeField] private Vector3 _destPos;
    private Vector3 _velocity;
    public Vector3 Velocity { get => _velocity;}

    public bool IsMove { get => _isMove; }

    void Update()
    {
        if (!_isMove)
            return;

        _velocity = _destPos - transform.position;
        _velocity.z = 0f;
        
        transform.position += _velocity.normalized * _speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, _destPos) < _stopDistance)
        {
            _isMove = false;
            transform.position = _destPos;
        }
    }

    public void Init(float speed)
    {
        _speed = speed;
        Stop();
    }

    public void Move(Vector3 dest)
    {
        _destPos = dest;
        _destPos.z = 0;
        _isMove = true;
    }

    public void Stop()
    {
        _isMove = false;
        _velocity = Vector3.zero;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
