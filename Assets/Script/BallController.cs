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

        // ������������
        feedbackObject = new GameObject("FeedbackObject");
        feedbackObject.transform.parent = transform;
        feedbackObject.transform.localPosition = Vector3.zero;
        feedbackObject.transform.localScale = Vector3.zero;

        // ��Ӿ�����Ⱦ�����
        feedbackSpriteRenderer = feedbackObject.AddComponent<SpriteRenderer>();
        feedbackSpriteRenderer.sprite = sprite; // �� YourSprite �滻Ϊ��ľ���ͼ
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

            // ������ק���ȵ������������λ�ú�����
            float distance = Mathf.Min(dragForce.magnitude/20, maxDistance);
            float scale = Mathf.Min(dragForce.magnitude/5 / maxDistance, maxScale);
            feedbackObject.transform.position = transform.position + dragForce.normalized * distance;
            feedbackObject.transform.localScale = new Vector3(scale, scale, 1f);
        }

        if (Input.GetMouseButtonUp(0) && isClicked)
        {
            rb.AddForce(dragForce, ForceMode2D.Impulse);

            // ���÷��������λ�ú�����
            feedbackObject.transform.localPosition = Vector3.zero;
            feedbackObject.transform.localScale = Vector3.zero;

            isClicked = false;
        }
    }
}