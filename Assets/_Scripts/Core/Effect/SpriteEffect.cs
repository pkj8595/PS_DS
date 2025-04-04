using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SpriteEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRendererZ;
    [SerializeField] private SpriteRenderer spriteRendererX;
    [SerializeField] private SpriteRenderer spriteRendererY;
    [SerializeField] private Sprite[] _sprites; // 애니메이션에 사용할 스프라이트 배열
    [SerializeField] private int _frameRate = 100; // 프레임 간 전환 속도
    private bool _isLoop = false;

    private System.Threading.CancellationTokenSource _taskSource;

    public string spriteSheetName; // 스프라이트 시트의 이름

    void Start()
    {
        // Resources 폴더에서 모든 스프라이트 로드
        Sprite[] sprites = Resources.LoadAll<Sprite>(spriteSheetName);
        _sprites = sprites;

        PlayEffect(transform.position, true);
    }


    public void PlayEffect(Vector3 position, bool isLoop, int frameRate = 100)
    {
        transform.position = position;
        _isLoop = isLoop;
        _frameRate = frameRate;
        gameObject.SetActive(true);
        StartTask();

    }

    public void StopEffect()
    {

    }

    private void StartTask()
    {
        CancelTask();
        _taskSource = new();
        CheckPawnPosition().Forget();
    }

    private void CancelTask()
    {
        if (_taskSource != null)
        {
            _taskSource.Cancel();
            _taskSource.Dispose();
            _taskSource = null;
        }
    }

    async UniTaskVoid CheckPawnPosition()
    {
        await UniTask.NextFrame();

        if (_isLoop)
        {
            //루프일 경우
            int i = 0;
            while (true)
            {
                spriteRendererX.sprite = _sprites[i];
                spriteRendererY.sprite = _sprites[i];
                spriteRendererZ.sprite = _sprites[i];
                    
                i++;
                if (!(i < _sprites.Length))
                {
                    i = 0;
                }
                await UniTask.Delay(_frameRate, cancellationToken: _taskSource.Token);
            }
            
        }
        else
        {
            for (int i = 0; i < _sprites.Length; i++)
            {
                spriteRendererX.sprite = _sprites[i];
                spriteRendererY.sprite = _sprites[i];
                spriteRendererZ.sprite = _sprites[i];

                await UniTask.Delay(_frameRate, cancellationToken: _taskSource.Token);
            }
        }
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelTask();
    }

}
