using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents an EdgeNode in a NodeTree and holds an edge its parent and sons
public class EdgeNode
{

    Edge edge;
    EdgeNode parent;
    EdgeNode leftSon;
    EdgeNode rightSon;
    bool hasSons;

    public EdgeNode(Edge e)
    {
        edge = e;
    }

    public EdgeNode(EdgeNode p, Edge e)
    {
        parent = p;
        edge = e;
    }

    public List<EdgeNode> getSons()
    {
        List<EdgeNode> list = new List<EdgeNode>();
        list.Add(leftSon);
        list.Add(rightSon);
        return list;
    }

    public bool getHasSons()
    {
        return hasSons;
    }

    public EdgeNode getParent()
    {
        return parent;
    }

    public Edge getEdge()
    {
        return edge;
    }

    public void setSons(EdgeNode enl, EdgeNode enr)
    {
        leftSon = enl;
        rightSon = enr;
        hasSons = true;
    }

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

    public List<EdgeNode> getLeaveNodes()
    {
        List<EdgeNode> leaves = new List<EdgeNode>();

        if (!leftSon.getHasSons())
        {
            leaves.Add(leftSon);
        }
        else
        {
            leftSon.getLeaveNodes();
        }

        if (!rightSon.getHasSons())
        {
            leaves.Add(rightSon);
        }
        else
        {
            rightSon.getLeaveNodes();
        }
        return leaves;
    }
}
