using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class holds the List structure that contain the edges of the model and its history
public class EdgeBinaryTreeList
{
    List<EdgeBinaryTree> edgeBTL;
    public EdgeBinaryTreeList()
    {
        edgeBTL = new List<EdgeBinaryTree>();
    }

    public void addEdge(Edge e)
    {
        EdgeBinaryTree edgeBT = new EdgeBinaryTree(e);
        edgeBTL.Add(edgeBT);
    }

    public List<Edge> getEdges()
    {
        List<Edge> list = new List<Edge>();
        for (int i = 0; i < edgeBTL.Count; i++)
        {
            list.AddRange(edgeBTL[i].getLeaves());
        }
        return list;
    }

    public List<EdgeNode> getLeaveNodes()
    {
        List<EdgeNode> list = new List<EdgeNode>();
        for (int i = 0; i < edgeBTL.Count; i++)
        {
            list.AddRange(edgeBTL[i].getLeaveNodes());
        }
        return list;
    }
}

