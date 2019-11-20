using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents a face of the model
public class Face: Element
{
    List<Vertex> vList;
    List<Edge> eList;

    public Face(List<Vertex> vl, int s) : base(s)
    {
        vList = vl;
    }

    public List<Vertex> getVertices()
    {
        return vList;
    }

    public bool has(Vertex v)
    {
        for (int i = 0; i < vList.Count; i++)
        {
            if (v == vList[i])
            {
                return true;
            }
        }
        return false;
    }
}
