using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class holds the Tree structure that contain the edges of the model and its history
public class EdgeBinaryTree
{

    EdgeNode root;

    public EdgeBinaryTree(Edge e)
    {
        root = new EdgeNode(e);
    }
    public List<Edge> getLeaves()
    {
        List<Edge> leaves = new List<Edge>();
        leaves.AddRange(root.getLeaves());
        return leaves;
    }
    public List<EdgeNode> getLeaveNodes()
    {
        List<EdgeNode> list = new List<EdgeNode>();

        if (!root.getHasSons())
        {
            list.Add(root);
            return list;
        }
        else
        {
            list.AddRange(root.getLeaveNodes());
        }
        return list;
    }
}

