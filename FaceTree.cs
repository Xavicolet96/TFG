using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class holds the Tree structure that contain the faces of the model and its history
public class FaceTree
{
    FaceNode root;

    public FaceTree()
    {
    }

    public void setRoot(Face f)
    {
        root = new FaceNode(f);
    }

    public List<Face> getFaces()
    {
        List<Face> faces = new List<Face>();
        faces.AddRange(root.getLeaves());
        return faces;
    }

    public List<FaceNode> getFaceNodes()
    {
        List<FaceNode> list = new List<FaceNode>();
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
