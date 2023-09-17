using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuaHua : MonoBehaviour
{
    public DotCilck _dot;

    

    void Update()
    {
   
       gameObject.GetComponent<Animator>().SetFloat("JinDu",_dot.DianJiJinDu);
    }
}
