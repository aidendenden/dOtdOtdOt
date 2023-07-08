using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IncrementalDisplay : MonoBehaviour
{
    public List<GameObject> image;
    private int index = 0;
    
    public void IncrementalImage()
    {
        image[index].SetActive(true);
        index++;
    }
}