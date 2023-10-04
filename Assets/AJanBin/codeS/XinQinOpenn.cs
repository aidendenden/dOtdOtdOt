using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XinQinOpenn : MonoBehaviour
{

    public bool XingQin1 = false;
    public bool XingQin2 = false;
    public bool XingQin3 = false;
    public bool XingQin4 = false;

    public GameObject XingQinOne;
    public GameObject XingQinTwo;
    public GameObject XingQinThree;
    public GameObject XingQinFour;

    // Update is called once per frame
    void Update()
    {
        XingQinOne.SetActive(XingQin1);
        XingQinTwo.SetActive(XingQin2);
        XingQinThree.SetActive(XingQin3);
        XingQinFour.SetActive(XingQin4);
    }
}
