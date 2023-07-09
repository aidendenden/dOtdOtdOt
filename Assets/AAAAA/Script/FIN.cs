using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FIN : MonoBehaviour
{
    public bool A = false;


    // Update is called once per frame
    void Update()
    {
        if (A)
        {
            A = false;
            SceneManager.LoadScene("StartScence");

        }
    }
}
