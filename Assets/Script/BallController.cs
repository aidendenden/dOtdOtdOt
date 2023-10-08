using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isClicked = false;
    private Vector3 clickPosition;
    private Vector3 dragForce;
    private GameObject feedbackObject;
    private SpriteRenderer feedbackSpriteRenderer;

    public float maxScale = 2f;
    public float maxDistance = 10f;

    public float FX = 0.15f;

    public Sprite sprite;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 创建反馈对象
        feedbackObject = new GameObject("FeedbackObject");
        feedbackObject.transform.parent = transform;
        feedbackObject.transform.localPosition = Vector3.zero;
        feedbackObject.transform.localScale = Vector3.zero;

        // 添加精灵渲染器组件
        feedbackSpriteRenderer = feedbackObject.AddComponent<SpriteRenderer>();
        feedbackSpriteRenderer.sprite = sprite; // 将 YourSprite 替换为你的精灵图
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickPosition = Input.mousePosition;
            isClicked = true;
        }

        if (Input.GetMouseButton(0) && isClicked)
        {
            Vector3 currentPosition = Input.mousePosition;
            dragForce = (currentPosition - clickPosition) * FX;

            // 根据拖拽力度调整反馈对象的位置和缩放
            float distance = Mathf.Min(dragForce.magnitude/20, maxDistance);
            float scale = Mathf.Min(dragForce.magnitude/5 / maxDistance, maxScale);
            feedbackObject.transform.position = transform.position + dragForce.normalized * distance;
            feedbackObject.transform.localScale = new Vector3(scale, scale, 1f);
        }

        if (Input.GetMouseButtonUp(0) && isClicked)
        {
            rb.AddForce(dragForce, ForceMode2D.Impulse);

            // 重置反馈对象的位置和缩放
            feedbackObject.transform.localPosition = Vector3.zero;
            feedbackObject.transform.localScale = Vector3.zero;

            isClicked = false;
        }
    }
}