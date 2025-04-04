using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioClip audioClip2;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Managers.Sound.PlayEffectPosition(audioClip, transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            Managers.Sound.PlayEffectPosition("Button_2_Pack2", transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //AudioSource audio = GetComponent<AudioSource>();
        //audio.PlayOneShot(audioClip);
        //audio.PlayOneShot(audioClip2);

        //float lifeTime = Mathf.Max(audioClip.length, audioClip2.length);
        //GameObject.Destroy(gameObject, lifeTime);
        Managers.Sound.Play(audioClip, Define.Sound.Effect);
        Managers.Sound.Play(audioClip2, Define.Sound.Effect);

    }

}
