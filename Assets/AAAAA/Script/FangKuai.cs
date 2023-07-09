using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FangKuai : MonoBehaviour
{
    public GameObject handxia;
    public Animator _a;


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.F))
        {
            handxia.active = true;
            _a.SetTrigger("st");
        }
    }
}
