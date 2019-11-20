using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents a FaceNode in a FaceTree and holds a face its parent and sons
public class FaceNode
{
    Face face;
    FaceNode parent;
    List<FaceNode> sons;
    bool hasSons;

    public FaceNode(Face f)
    {
        sons = new List<FaceNode>();
        face = f;
        hasSons = false;
    }

    public FaceNode(FaceNode p, Face f)
    {
        sons = new List<FaceNode>();
        face = f;
        parent = p;
    }

    public void addSon(FaceNode fn)
    {
        sons.Add(fn);
        hasSons = true;
    }

    public Face getFace()
    {
        return face;
    }

    public FaceNode getParent()
    {
        return parent;
    }

    public bool getHasSons()
    {
        return hasSons;
    }

    public List<Face> getLeaves()
    {
        List<Face> leaves = new List<Face>();
        if (hasSons)
        {
            for (int i = 0; i < sons.Count; i++)
            {
                leaves.AddRange(sons[i].getLeaves());
            }
        }
        else
        {
            leaves.Add(face);
            return leaves;
        }
        return leaves;
    }
    public List<FaceNode> getLeaveNodes()
    {
        List<FaceNode> leaves = new List<FaceNode>();
        for (int i = 0; i < sons.Count; i++)
        {
            if (!sons[i].getHasSons())
            {
                leaves.Add(sons[i]);
            }
            else
            {
                sons[i].getLeaveNodes();
            }
        }
        return leaves;
    }

}