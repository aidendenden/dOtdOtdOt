using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FangKuai : MonoBehaviour
{
    public GameObject handxia;
    public Animator _a;
    public Animator _aaaas;


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.F)|| Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C))
        {
            
            _a.SetTrigger("st");
            _aaaas.SetTrigger("st");
        }
    }
}
