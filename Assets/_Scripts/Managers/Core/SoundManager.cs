using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// mp3 player -> AudioSource
// 음원 -> AudioClip
// 듣는이 -> AudioListener
public class SoundManager : ManagerBase
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.Count];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    [Range(0.0f, 1.0f)]
    float effectVolume = 1.0f;

    [Range(0.0f, 1.0f)]
    float bgmVolume = 1.0f;

    public override void Init()
    {
        base.Init();
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundName = System.Enum.GetNames(typeof(Define.Sound));
            for (int i = 0; i < soundName.Length -1; i++)
            {
                GameObject go = new GameObject { name = soundName[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Define.Sound.Bgm].loop = true;

        }
    }

    public override void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip clip =  GetOrAddAudioClip(path, type);
        Play(clip, type, pitch);
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.volume = bgmVolume;
            audioSource.Play();
        }
        else
        {
           
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.volume = effectVolume;
            audioSource.PlayOneShot(audioClip);
        }
    }
    public void PlayEffectPosition(string path, Vector3 position)
    {
        AudioClip clip = GetOrAddAudioClip(path, Define.Sound.Effect);
        PlayEffectPosition(clip, position);
    }

    public void PlayEffectPosition(AudioClip audioClip, Vector3 position)
    {
        if (audioClip == null)
            return;

        AudioSource.PlayClipAtPoint(audioClip, position, effectVolume);
    }

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip;
        if (type == Define.Sound.Bgm)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }
        else
        {
            if (_audioClips.TryGetValue(path, out audioClip) is false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }

}
