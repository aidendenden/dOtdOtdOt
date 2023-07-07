using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenceChange : MonoBehaviour
{
    public bool isFinshed = false;



    public void loadStartScence()
    {
        SceneManager.LoadScene("StartScence");
    }

    public void loadMainScence()
    {
        SceneManager.LoadScene("MainScence");
    }

    public void loadEndScence()
    {
        SceneManager.LoadScene("EndScence");
    }



    private void Update()
    {
        ifFinshedgoToMain();

    }



    private void ifFinshedgoToMain()
    {
        if (isFinshed&&gameObject.CompareTag("ZhuanChang"))
        {
            loadMainScence();
            isFinshed = false;
        }
    }
}
