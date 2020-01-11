using System;
using System.Collections.Generic;

// This class holds the List structure that contain the edges of the model and its history
[Serializable()]
public class EdgeBinaryTreeList
{
    public List<EdgeBinaryTree> edgeBTL;
    public int count;

    // Initializes the tree list
    public EdgeBinaryTreeList()
    {
        edgeBTL = new List<EdgeBinaryTree>();
    }

    // Adds an edge and its corresponding tree
    public void addEdge(Edge e)
    {
        EdgeBinaryTree edgeBT = new EdgeBinaryTree(e);
        edgeBTL.Add(edgeBT);
    }

    // Gets all active edges
    public List<Edge> getEdges()
    {
        List<Edge> list = new List<Edge>();
        for (int i = 0; i < edgeBTL.Count; i++)
        {
            list.AddRange(edgeBTL[i].getLeaves());
        }
        return list;
    }

    // Gets all active edges in step n
    public List<Edge> getEdges(int n)
    {
        List<Edge> list = new List<Edge>();
        for (int i = 0; i < edgeBTL.Count; i++)
        {
            if (edgeBTL[i].root.edge.step <= n)
            {
                list.AddRange(edgeBTL[i].getLeaves(n));
            }
        }
        return list;
    }

    // Gets all active EdgeNodes
    public List<EdgeNode> getLeaveNodes()
    {
        List<EdgeNode> list = new List<EdgeNode>();
        for (int i = 0; i < edgeBTL.Count; i++)
        {
            list.AddRange(edgeBTL[i].getLeaveNodes());
        }
        return list;
    }

    override
    public string ToString()
    {
        string s = "";
        List<Edge> e = getEdges();
        for (int i = 0; i < e.Count; i++)
        {
            s += e[i].ToString();
        }
        return s;
    }
}