using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UIElements;

public class MouseClickToMove : MonoBehaviour
{
    private Seeker _Seeker;
    private List<Vector3> _PathPointList;
    private int _CurrentIndex = 0;
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        _Seeker = GetComponent<Seeker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main == null)
                return;

            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0;

            CreataPath(target);
        }

        MouseMove();
    }

    private void MouseMove()
    {
        if ( _PathPointList == null|| _PathPointList.Count <= 0 )
            return;
        if (Vector2.Distance(transform.position, _PathPointList[_CurrentIndex]) > 0.2f)
        {
            Vector3 dir = (_PathPointList[_CurrentIndex] - transform.position).normalized;

            transform.position += dir * Time.deltaTime * speed;
        }
        else
        {
            if (_CurrentIndex == _PathPointList.Count - 1)
                return;

            _CurrentIndex++;
        }
    }

    private void CreataPath(Vector3 target)
    {
        _CurrentIndex = 0;
        _Seeker.StartPath(transform.position, target, path => { _PathPointList = path.vectorPath; });
    }
}