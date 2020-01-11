using System;
using System.Collections.Generic;

// This class represents a FaceNode in a FaceTree and holds a face its parent and sons
[Serializable()]
public class FaceNode
{
    public Face face;
    public FaceNode parent;
    public List<FaceNode> sons;
    public bool hasSons;

    // Constructs a face node
    public FaceNode(Face f)
    {
        sons = new List<FaceNode>();
        face = f;
        hasSons = false;
    }

    // Constructs a face node and sets its parent
    public FaceNode(FaceNode p, Face f)
    {
        sons = new List<FaceNode>();
        face = f;
        parent = p;
        p.addSon(this);
    }

    // Adds a son to the node
    public void addSon(FaceNode fn)
    {
        sons.Add(fn);
        hasSons = true;
    }

    // Returns the faces that stem from the node
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

    // Returns the faces that stem from the node
    public List<Face> getLeaves(int n)
    {
        List<Face> leaves = new List<Face>();
        if (hasSons)
        {
            for (int i = 0; i < sons.Count; i++)
            {
                if (sons[i].face.step<=n)
                {
                    leaves.AddRange(sons[i].getLeaves(n));
                }
            }
        }
        else
        {
            leaves.Add(face);
            return leaves;
        }
        return leaves;
    }

    // Retruns the FaceNodes that stem from the node
    public List<FaceNode> getLeaveNodes()
    {
        List<FaceNode> leaves = new List<FaceNode>();
        for (int i = 0; i < sons.Count; i++)
        {
            if (!sons[i].hasSons)
            {
                leaves.Add(sons[i]);
            }
            else
            {
                leaves.AddRange(sons[i].getLeaveNodes());
            }
        }
        return leaves;
    }

    // Given a face it returns its node
    public FaceNode findNode(Face f)
    {
        if (this.hasSons)
        {
            for (int i = 0; i < sons.Count; i++)
            {
                if (sons[i].face == f)
                {
                    return sons[i];
                }
                else
                {
                    sons[i].findNode(f);
                }
            }
        }
        return null;
    }
}