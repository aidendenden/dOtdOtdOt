using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XiaShou : MonoBehaviour
{
    public bool A;
    public GameObject huakuai;

    private void Update()
    {
        if (A)
        {
            A = false;
            huakuai.SetActive(true);
        }
    }
}
