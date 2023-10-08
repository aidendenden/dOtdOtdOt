using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GoldMiner : MonoBehaviour
{
    public GameObject hookPrefab;
    public Transform hookSpawnPoint;
    public Transform hookAttachPoint;
    public float hookSpeed = 10f;
    public float hookRetractSpeed = 5f;

    public bool back = false;
    public bool Have = false;

    public LoopMove loopMove;

    private GameObject currentHook;
    private bool isHookMoving;
    private bool isHookRetracting;
    private bool isHookAttached;

    private void Start()
    {
        loopMove = GameObject.FindGameObjectWithTag("mineA").GetComponent<LoopMove>();
       
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isHookMoving && !isHookRetracting)
            {

                back = false;
                Have = false;
                LaunchHook();
            }
            else if (isHookAttached)
            {
                RetractHook();
            }
        }
    }

    void LaunchHook()
    {
        loopMove.iscanMove = false;
        currentHook = Instantiate(hookPrefab, hookSpawnPoint.position, Quaternion.identity);
        Vector3 direction = hookAttachPoint.position - currentHook.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentHook.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        StartCoroutine(MoveHook());
    }

    IEnumerator MoveHook()
    {
        isHookMoving = true;
        isHookAttached = false;

        while (currentHook.transform.position != hookAttachPoint.position)
        {
            currentHook.transform.position = Vector3.MoveTowards(currentHook.transform.position, hookAttachPoint.position, hookSpeed * Time.deltaTime);
            yield return null;
        }

        while (currentHook.transform.position != hookSpawnPoint.position)
        {
            currentHook.transform.position = Vector3.MoveTowards(currentHook.transform.position, hookSpawnPoint.position, hookRetractSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(currentHook);
        loopMove.iscanMove = true;
        isHookMoving = false;
    }

    void RetractHook()
    {
        isHookRetracting = true;
        isHookAttached = false;
    }
}