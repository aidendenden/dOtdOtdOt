using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flutter : MonoBehaviour
{  
    public List<GameObject> image = new List<GameObject>();

    List<int> count = null;
    
    public void FlutterImage(int index)
    {
        if (index>=image.Count)
        {
            return;
        }
        for (int i = 0; i < index; i++)
        {
            var n =RandomCount();
            image[n].SetActive(true);
            StartCoroutine(SetShow(n));
        }
    }

    public int  RandomCount()
    {
        if (count.Count>5)
        {
            count.Clear();
        }
        var num=Random.Range(0, 7);
        while (count.Contains(num))
        {
            num = Random.Range(0, 7);
        }

        count.Add(num);
        return num;
    }
    
    IEnumerator SetShow(int i)
    {
        yield return new WaitForSeconds(4.0f);
        image[i].SetActive(false);
    }
}
