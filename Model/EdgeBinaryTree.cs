using System;
using System.Collections.Generic;

// This class holds the Tree structure that contain the edges of the model and its history
[Serializable()]
public class EdgeBinaryTree
{
    public EdgeNode root;

    // Initializes the tree setting the edge as the root
    public EdgeBinaryTree(Edge e)
    {
        root = new EdgeNode(e);
    }

    // Returns active Edges
    public List<Edge> getLeaves()
    {
        List<Edge> leaves = new List<Edge>();
        leaves.AddRange(root.getLeaves());
        return leaves;
    }

    // Returns active Edges in step n
    public List<Edge> getLeaves(int n)
    {
        List<Edge> leaves = new List<Edge>();
        leaves.AddRange(root.getLeaves(n));
        return leaves;
    }

    // Returns active EdgeNodes
    public List<EdgeNode> getLeaveNodes()
    {
        List<EdgeNode> list = new List<EdgeNode>();

        if (!root.hasSons)
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