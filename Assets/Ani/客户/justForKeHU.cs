using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class justForKeHU : MonoBehaviour
{
    public GameObject KeLi;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag =="Player")
        {
            gameObject.GetComponent<Animator>().SetTrigger("PENG");
            gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip);
            GameObject game = GameObject.Instantiate(KeLi,gameObject.transform.position,Quaternion.identity);
        }
    }
}
