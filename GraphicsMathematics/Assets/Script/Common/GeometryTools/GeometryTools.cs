using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpaceRelationShip
{
    Invalid = -1,
    Intersect = 0,//相交
    NonIntersect = 1,//不相交
    Parallel = 2,//平行
    Inner = 3,//在内部
    Outter = 4,//外部
}

public static class GeometryTools
{
    /// <summary>
    /// 求直线与平面的交点
    /// </summary>
    /// <param name="planeNormal">平面法向量</param>
    /// <param name="planePoint">已知平面上一点</param>
    /// <param name="lineDirection">直线的方向</param>
    /// <param name="linePoint">已知直线上一点</param>
    /// <returns></returns>
    public static SpaceRelationShip LineIntersectWithPlanePoint(Vector3 planeNormal, Vector3 planePoint, Vector3 lineDirection, Vector3 linePoint)
    {
        //平面方程Ax+By+Zc+D = 0，法向量n = (A,B,C)，P0(x0,y0,z0)为平面上一点
        //直线方程P = L0 + Ld，L0为直线上一点，L为直线的方向
        //求交点
        //P-P0与n垂直，有(P-P0)· n = 0 ，直线代入得到
        //(L0+Ld-P0)· n = 0得到
        //Ld· n = （P0-L0）· n=>d = (P0-L0)· n/L· n
        //代入直线方程得到交点
        if (Mathf.Abs(Vector3.Dot(lineDirection, planeNormal)) > 0)//相交
        {
            return SpaceRelationShip.Intersect;
        }
        else if (Mathf.Abs(Vector3.Dot(linePoint - planePoint, planeNormal)) > 0)//在平面内
        {
            return SpaceRelationShip.Inner;
        }
        else
            return SpaceRelationShip.Parallel;
    }

    /// <summary>
    /// 求直线与平面的交点
    /// </summary>
    /// <param name="planeNormal">平面法向量</param>
    /// <param name="planePoint">已知平面上一点</param>
    /// <param name="lineDirection">直线的方向</param>
    /// <param name="linePoint">已知直线上一点</param>
    /// <returns></returns>
    public static Vector3 GetLineIntersectWithPlanePoint(Vector3 planeNormal, Vector3 planePoint, Vector3 lineDirection, Vector3 linePoint)
    {
        //平面方程Ax+By+Zc+D = 0，法向量n = (A,B,C)，P0(x0,y0,z0)为平面上一点
        //直线方程P = L0 + Ld，L0为直线上一点，L为直线的方向
        //求交点
        //P-P0与n垂直，有(P-P0)· n = 0 ，直线代入得到
        //(L0+Ld-P0)· n = 0得到
        //Ld· n = （P0-L0）· n=>d = (P0-L0)· n/L· n
        //代入直线方程得到交点
        float d = Vector3.Dot(planePoint - linePoint, planeNormal) / Vector3.Dot(lineDirection, planeNormal);
        Vector3 intersectPoint = linePoint + lineDirection * d;
        return intersectPoint;
    }

    /// <summary>
    /// 求直线与平面的交点
    /// </summary>
    /// <param name="planeNormal">平面法向量</param>
    /// <param name="planePoint">已知平面上一点</param>
    /// <param name="rayDirection">射线的方向</param>
    /// <param name="rayPoint">射线起点</param>
    /// <returns></returns>
    public static SpaceRelationShip RayIntersectWithPlanePoint(Vector3 planeNormal, Vector3 planePoint, Vector3 rayDirection, Vector3 rayPoint)
    {
        //平面方程Ax+By+Zc+D = 0，法向量n = (A,B,C)，P0(x0,y0,z0)为平面上一点
        //直线方程P = L0 + Ld，L0为射线起点，L为射线的方向
        //求交点
        //P-P0与n垂直，有(P-P0)· n = 0 ，射线代入得到
        //(L0+Ld-P0)· n = 0得到
        //Ld· n = （P0-L0）· n=>d = (P0-L0)· n/L· n
        //d >= 0相交 d <0不相交
        float d = Vector3.Dot(planePoint - rayPoint, planeNormal) / Vector3.Dot(rayDirection, planeNormal);
        if (d > 0)
            return SpaceRelationShip.Intersect;
        else 
            return SpaceRelationShip.NonIntersect;
    }

}
