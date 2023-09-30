using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MangeManger : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 1f)]
    public float Hard = 1;

    public int numberOne = 1;
    public int numberTwo = 1;


    public float shrinkThreshold = 50f;
    public float moveSpeed = 50f;
    public float shrinkSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moveSpeed - shrinkSpeed * Time.deltaTime > shrinkThreshold)
        {
            moveSpeed -= shrinkSpeed * Time.deltaTime;
        }
    }
}
