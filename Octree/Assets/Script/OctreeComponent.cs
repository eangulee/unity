using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class OctreeComponent : MonoBehaviour
{

    public float size = 5;
    public int depth = 2;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        var octree = new Octree<int>(this.transform.position, size, depth);

        DrawNode(octree.GetRoot());
    }

    private Color minColor = new Color(1, 1, 1, 1f);
    private Color maxColor = new Color(0, 0.5f, 1, 0.25f);

    private void DrawNode(Octree<int>.OctreeNode<int> node, int nodeDepth = 0)
    {
        if (!node.IsLeaf())
        {
            foreach (var subnode in node.Nodes)
            {
                DrawNode(subnode, nodeDepth + 1);
            }
        }
        Gizmos.color = Color.Lerp(minColor, maxColor, nodeDepth / (float)depth);
        Gizmos.DrawWireCube(node.Position, Vector3.one * node.Size);
    }
}