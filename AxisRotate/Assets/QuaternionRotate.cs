using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionRotate : MonoBehaviour
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
        float theta = rotateSpeed * Time.fixedTime % 360;
        Quaternion quaternion = GetQuaternion(theta);
        this.transform.position = quaternion * _originalPos;
    }

    /// <summary>
    /// 获取旋转四元数
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    private Quaternion GetQuaternion(float theta)
    {
        /*
        w = cos(theta / 2)
        x = ax * sin(theta / 2)
        y = ay * sin(theta / 2)
        z = az * sin(theta / 2)
        */
        float sin = Mathf.Sin(theta / 2f);
        float cos = Mathf.Cos(theta / 2f);
        Quaternion quaternion = new Quaternion();
        quaternion.w = cos;
        quaternion.x = axis.x * sin;
        quaternion.y = axis.y * sin;
        quaternion.z = axis.z * sin;
        return quaternion;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, axis * 5f);
    }
}
