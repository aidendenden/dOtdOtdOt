using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IncrementalDisplay : MonoBehaviour
{
    public List<GameObject> image = new List<GameObject>();
    private int index = 0;
    
    public void IncrementalImage()
    {
        if (index>=image.Count)
        {
            return;
        }
        image[index].SetActive(true);
        index++;
    }
}