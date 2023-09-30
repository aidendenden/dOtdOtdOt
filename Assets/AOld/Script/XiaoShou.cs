using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class XiaoShou : MonoBehaviour
{
    public bool A = false;

    public float speedUp = 10f;

    private MangeManger mangerManger;

    // Update is called once per frame
    void Update()
    {
        if (A)
        {
            A = false;
            GameObject.Destroy(gameObject);
        }
    }

    private void Start()
    {
        mangerManger = GameObject.FindGameObjectWithTag("Manger").GetComponent<MangeManger>();
        mangerManger.moveSpeed += speedUp;
    }
}
