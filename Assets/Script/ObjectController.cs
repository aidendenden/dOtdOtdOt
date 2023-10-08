using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public GameObject objectToToggle;
    public GameObject objectToToggleT;
    public GameObject objectToToggleTT;
    public float toggleDelay = 2f;

    private bool isObjectActive = true;

    public void ToggleObject()
    {
        if (isObjectActive)
        {
            objectToToggle.SetActive(false);
            objectToToggleT.SetActive(false);
            objectToToggleTT.SetActive(false);
            Invoke("ToggleObjectAfterDelay", toggleDelay);
        }
    }

    private void ToggleObjectAfterDelay()
    {
        objectToToggle.SetActive(true);
        objectToToggleT.SetActive(true);
        objectToToggleTT.SetActive(true);
        isObjectActive = true;
    }
}