using System;
using System.Collections.Generic;

// This class holds the Tree structure that contain the faces of the model and its history
[Serializable()]
public class FaceTree
{
    public FaceNode root;
    public int count;
    public FaceTree()
    {
    }
    // Given a face it creates a FaceNode and sets it as a root
    public void setRoot(Face f)
    {
        root = new FaceNode(f);
    }

    // Returns a list of the active faces
    public List<Face> getFaces()
    {
        List<Face> faces = new List<Face>();
        faces.AddRange(root.getLeaves());
        return faces;
    }

    public List<Face> getFaces(int n)
    {
        List<Face> faces = new List<Face>();
        faces.AddRange(root.getLeaves(n));
        return faces;
    }

    // Returns a list of the active FaceNodes
    public List<FaceNode> getFaceNodes()
    {
        List<FaceNode> list = new List<FaceNode>();
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

    // Given a face finds its node in the tree
    public FaceNode findNode(Face f)
    {
        if (root.face==f)
        {
            return root;
        }
        else
        {
            root.findNode(f);
        }
        return null;
    }
}