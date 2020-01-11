using System;
using System.Collections.Generic;
using UnityEngine;

// This class represents a face of the model
[Serializable()]
public class Face: Element
{
    public List<VertexNode> vList;
    public List<VertexNode> extraV;
    public Vector3 normal;

    // Given at least 3 vertices it creates a face 
    public Face(List<VertexNode> vl, int s, int id) : base(s, id)
    {
        extraV = new List<VertexNode>();
        vList = vl;
        Vector3 p1, p2, p3, v1, v2;
        if (vl.Count >= 3)
        {
            p1 = vl[0].getPosition();
            p2 = vl[1].getPosition();
            p3 = vl[2].getPosition();

            v1 = p2 - p1;
            v2 = p3 - p1;

            normal = Vector3.Cross(v1, v2);
            normal.Normalize();
        }
        else
        {
            Debug.Log("Algo falla calculant la normal------------------------");
            normal = new Vector3(0, 0, -1);
        }
    }

    // Checks if the face has certain vertex
    public bool has(VertexNode v)
    {
        for (int i = 0; i < vList.Count; i++)
        {
            if (v == vList[i])
            {
                return true;
            }
        }

        for (int i = 0; i < extraV.Count; i++)
        {
            if (v == extraV[i])
            {
                return true;
            }
        }
        return false;
    }


    public void sortVertices()
    {
        Face f = this;
        Debug.Log(f.id);
        List<VertexNode> old = f.vList;
        int n = f.vList.Count;
        List<VertexNode> sorted = new List<VertexNode>();
        VertexNode v = old[0];

        old.Remove(v);
        sorted.Add(v);

        for (int i = 0; i < n; i++)
        {
            List<Edge> eL = v.edges;
            for (int j = 0; j < eL.Count; j++)
            {
                if(eL[j].end != v && old.Contains(eL[j].end))
                {
                    v = eL[j].end;
                    sorted.Add(v);
                    old.Remove(v);
                    break;
                }
                else if (eL[j].origin != v && old.Contains(eL[j].origin))
                {
                    v = eL[j].origin;
                    sorted.Add(v);
                    old.Remove(v);
                    break;
                }
            }
        }
        sorted.AddRange(old);
        Debug.Log(n == sorted.Count);
        f.vList = sorted;
    }

    override
    public string ToString()
    {
        string s = id + ": ";
        for (int i = 0; i < vList.Count; i++)
        {
            s += ' ';
            s += vList[i].id;
        }

        s += "   Extra: ";
        for (int i = 0; i < extraV.Count; i++)
        {
            s += ' ';
            s += extraV[i].id;
        }
        return s;
    }
}