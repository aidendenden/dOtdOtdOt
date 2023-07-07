using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenceChange : MonoBehaviour
{
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
}
