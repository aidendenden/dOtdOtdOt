using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class EyeSwitch : MonoBehaviour
{
    public Image image;
    public string[] imagePathList=
    {
        "Assets/AAAAA/Image/眼睛1.png",
        "Assets/AAAAA/Image/眼睛2.png",
        "Assets/AAAAA/Image/眼睛3.png",
        "Assets/AAAAA/Image/眼睛4.png",
        "Assets/AAAAA/Image/眼睛5.png",
        "Assets/AAAAA/Image/眼睛6.png",
        "Assets/AAAAA/Image/眼睛7.png",
        "Assets/AAAAA/Image/眼睛8.png",
        "Assets/AAAAA/Image/眼睛9.png",
        "Assets/AAAAA/Image/眼睛10.png"
    };
    
    public void changeImage(int index)
    {
        // 从本地文件加载图像数据
        byte[] imageData = File.ReadAllBytes(imagePathList[index]);

        // 创建新的 Texture2D 对象，并使用加载的图像数据初始化它
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);

        // 将 Texture2D 对象设置为 Image 的源图像
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}
