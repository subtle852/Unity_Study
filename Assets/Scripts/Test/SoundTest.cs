using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public AudioClip audioClip;
    public AudioClip audioClip2;

    //int i = 0;
    private void OnTriggerEnter(Collider other)
    {
        // 기본 실습
        //AudioSource audio = GetComponent<AudioSource>();
        //audio.PlayOneShot(audioClip);
        //audio.PlayOneShot(audioClip2);

        //float lifeTime = Mathf.Max(audioClip.length, audioClip2.length);
        //GameObject.Destroy(gameObject, lifeTime);

        // SoundManager Effect 실습
        //Managers.Sound.Play("UnityChan/univ0001");
        //Managers.Sound.Play("UnityChan/univ0002");

        // SoundManager Bgm 실습
        //i++;

        //if(i % 2 == 0 )
        //    Managers.Sound.Play("UnityChan/univ0001", Define.Sound.Bgm);
        //else
        //    Managers.Sound.Play("UnityChan/univ0002", Define.Sound.Bgm);

        // SoundManager audioClip 인자로 받는 Play 버전 실습
        //i++;

        //if (i % 2 == 0)
        //    Managers.Sound.Play(audioClip, Define.Sound.Bgm);
        //else
        //    Managers.Sound.Play(audioClip2, Define.Sound.Bgm);

        // 3D Sound 실습
        AudioSource audio = GetComponent<AudioSource>();
        audio.spatialBlend = 1.0f;
        audio.minDistance = 10.0f;
        audio.maxDistance = 20.0f;
        audio.clip = audioClip;
        audio.loop = true;
        audio.Play();

        AudioSource.PlayClipAtPoint(audioClip2, new Vector3(0, 0, 0));

    }
}
