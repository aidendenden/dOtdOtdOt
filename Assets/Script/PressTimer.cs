using UnityEngine;

public class PressTimer : MonoBehaviour
{
    public float requiredPressDuration = 2f; // 需要按住的时间
    private bool isPressing = false; // 判断是否正在按压
    private float pressStartTime; // 按压开始时间

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPressing = true;
            pressStartTime = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isPressing)
            {
                float pressDuration = Time.time - pressStartTime;
                if (pressDuration >= requiredPressDuration)
                {
                   
                    Debug.Log("按压持续时间: " + FormatTime(pressDuration));
                }
                else
                {
                    Debug.Log("未满足需要的按压持续时间: " + FormatTime(pressDuration));
                }
            }

            isPressing = false;
        }

        if (isPressing)
        {
            float pressDuration = Time.time - pressStartTime;
            if (pressDuration >= requiredPressDuration)
            {
                Debug.Log("已完成按压，但仍然在按压...");
            }
            else
            {
                var _time = FormatTime(pressStartTime - pressDuration);
                Debug.Log($"按压剩余时间{_time}");
            }
        }
    }
    
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

        return $"{minutes:00}:{seconds:00}";
    }
}