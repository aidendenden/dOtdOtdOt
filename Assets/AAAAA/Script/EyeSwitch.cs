using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class EyeSwitch : MonoBehaviour
{
    public bool A = false;
    public Image image;

    public Sprite[] imageList;
    
    public void changeImage(int index)
    {

        // 将 Texture2D 对象设置为 Image 的源图像
        image.sprite = imageList[index];

        RectTransform rectTransform = image.rectTransform;
        rectTransform.sizeDelta = new Vector2(image.sprite.rect.width, image.sprite.rect.height);

    }

    private void Update()
    {
        if (A)
        {
            A = false;
            changeImage(UnityEngine.Random.Range(0, 9));
        }
    }


}
