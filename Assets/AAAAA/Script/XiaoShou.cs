using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XiaoShou : MonoBehaviour
{
    public bool A = false;

    // Update is called once per frame
    void Update()
    {
        if (A)
        {
            A = false;
            GameObject.Destroy(gameObject);
        }
    }
}
