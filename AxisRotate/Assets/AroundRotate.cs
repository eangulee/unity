using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundRotate : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed = 1f;
    private Vector3 _axis;
    private Vector3 _originalPos;

    public Vector3 axis
    {
        get
        {
            return _axis;
        }
        set
        {
            _axis = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (target)
            axis = target.position;
        _originalPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
            axis = target.position.normalized;
        transform.RotateAround(Vector3.zero, axis, Time.deltaTime * 36f * rotateSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, axis * 5f);
    }
}
