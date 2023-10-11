using UnityEngine;

public class ActivateGameObject : MonoBehaviour
{
    //public KeyCode activationKey;
    public GameObject targetGameObject;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            ActivateTargetGameObject();
        }
    }

    private void ActivateTargetGameObject()
    {
        targetGameObject.SetActive(true);
    }
}