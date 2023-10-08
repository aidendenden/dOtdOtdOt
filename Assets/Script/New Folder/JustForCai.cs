using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustForCai : MonoBehaviour
{
    public AudioClip audioClip;
    public GoldMiner goldMiner;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "CaiPoint"&&goldMiner.Have ==false)
        {
            goldMiner.Have = true;
            goldMiner.back = true;
            gameObject.transform.parent = collision.gameObject.transform;
        }
    }

    private void OnDestroy()
    {
        GameObject lingshiaudio = new GameObject();
        lingshiaudio.AddComponent<AudioSource>();
        AudioSource _audioSource = lingshiaudio.GetComponent<AudioSource>();
        _audioSource.clip = audioClip;
        _audioSource.PlayOneShot(_audioSource.clip);
    }
}
