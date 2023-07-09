using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuaJianan : MonoBehaviour
{
    public GameObject shoushou;
    public bool A;
    public Animator aaaa;

    private void Update()
    {
        if (A)
        {
            A = false;
            shoushou.SetActive(true);
        }
    }

    public void aaaaa()
    {
        aaaa.SetTrigger("st");
    }

}
