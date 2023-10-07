using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    public Sprite[] sprites; // ����ͼƬ����

    private SpriteRenderer spriteRenderer; // ������Ⱦ�����

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // ���ѡ��һ�ž���ͼƬ
        int randomIndex = Random.Range(0, sprites.Length);
        Sprite randomSprite = sprites[randomIndex];

        // ��������ľ���ͼƬ
        spriteRenderer.sprite = randomSprite;
    }
}