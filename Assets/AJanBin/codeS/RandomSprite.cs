using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    public Sprite[] sprites; // 精灵图片数组

    private SpriteRenderer spriteRenderer; // 精灵渲染器组件

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // 随机选择一张精灵图片
        int randomIndex = Random.Range(0, sprites.Length);
        Sprite randomSprite = sprites[randomIndex];

        // 设置物体的精灵图片
        spriteRenderer.sprite = randomSprite;
    }
}