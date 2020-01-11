using System;
using System.Collections.Generic;

// This class represents an EdgeNode in a NodeTree and holds an edge its parent and sons
[Serializable()]
public class EdgeNode
{
    public Edge edge;
    public EdgeNode parent;
    public EdgeNode leftSon;
    public EdgeNode rightSon;
    public bool hasSons;

    // Sets the edge and the parent EdgeNode
    public EdgeNode(Edge e)
    {
        edge = e;
    }
    public EdgeNode(EdgeNode p, Edge e)
    {
        parent = p;
        edge = e;
    }

    // Sets the nodes sons
    public void setSons(EdgeNode enl, EdgeNode enr)
    {
        leftSon = enl;
        rightSon = enr;
        hasSons = true;
    }

    // Returns the active edges that stem from this node
    public List<Edge> getLeaves()
    {
        List<Edge> leaves = new List<Edge>();
        if (hasSons)
        {
            leaves.AddRange(leftSon.getLeaves());
            leaves.AddRange(rightSon.getLeaves());
        }
        else
        {
            leaves.Add(edge);
            return leaves;
        }
        return leaves;
    }

    // Returns the active in step n edges that stem from this node
    public List<Edge> getLeaves(int n)
    {
        List<Edge> leaves = new List<Edge>();
        if (hasSons)
        {
            if (leftSon.edge.step <= n)
            {
                leaves.AddRange(leftSon.getLeaves(n));
            }
            if (leftSon.edge.step <= n)
            {
                leaves.AddRange(rightSon.getLeaves(n));
            }
            
        }
        else
        {
            leaves.Add(edge);
            return leaves;
        }
        return leaves;
    }

    // Returns the active EdgeNodes that stem from this node
    public List<EdgeNode> getLeaveNodes()
    {
        List<EdgeNode> leaves = new List<EdgeNode>();

        if (!leftSon.hasSons)
        {
            leaves.Add(leftSon);
        }
        else
        {
            leaves.AddRange(leftSon.getLeaveNodes());
        }

        if (!rightSon.hasSons)
        {
            leaves.Add(rightSon);
        }
        else
        {
            leaves.AddRange(rightSon.getLeaveNodes());
        }
        return leaves;
    }

    override
    public string ToString()
    {
        return edge.ToString();
    }
}