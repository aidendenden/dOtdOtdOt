using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDelet : MonoBehaviour
{
   private void Start()
   {
      DontDestroyOnLoad(this.transform.gameObject);
   }
}
