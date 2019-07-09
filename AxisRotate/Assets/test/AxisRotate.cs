using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 任意轴旋转
/// </summary>
public class AxisRotate : MonoBehaviour
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
        Matrix4x4 matrix4X4 = GetRotateMatrix(theta);
        this.transform.position = matrix4X4 * _originalPos;
    }

    /// <summary>
    /// 获取任意轴选择矩阵
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    private Matrix4x4 GetRotateMatrix(float theta)
    {
        float sin = Mathf.Sin(theta);
        float cos = Mathf.Cos(theta);
        Matrix4x4 matrix4X4 = new Matrix4x4(
           new Vector4(cos + axis.x * axis.x * (1 - cos), axis.x * axis.y * (1 - cos) - axis.z * sin, axis.x * axis.z * (1 - cos) + axis.y * sin),
           new Vector4(axis.x * axis.y * (1 - cos) + axis.z * sin, cos + axis.y * axis.y * (1 - cos), axis.y * axis.z * (1 - cos) - axis.x * sin),
           new Vector4(axis.x * axis.z * (1 - cos) - axis.y * sin, axis.y * axis.z * (1 - cos) + axis.x * sin, cos + axis.z * axis.z * (1 - cos)),
           new Vector4(0, 0, 0, 1f)
           );
        return matrix4X4;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, axis * 5f);
    }
}
