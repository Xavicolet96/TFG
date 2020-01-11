using System;
using System.Collections.Generic;
using UnityEngine;

// This class represents an edge of the model
[Serializable()]
public class Edge : Element
{
    public VertexNode origin;
    public VertexNode end;
    public List<Face> faces;

    // Initializes both ends and the list of faces
    public Edge(VertexNode o, VertexNode e, List<Face> f, int s, int id) : base(s, id)
    {
        origin = o;
        end = e;
        faces = new List<Face>();
        faces.AddRange(f);
    }
    public Edge(VertexNode o, VertexNode e, int s, int id) : base(s, id)
    {
        origin = o;
        end = e;
        faces = new List<Face>();
    }

    // Checks if the given plane cuts the edge and returns the point if it does
    public Vector3 isCut(Plane p)
    {
        float t, para;
        para = (p.A * (end.getPosition().x - origin.getPosition().x) + p.B * (end.getPosition().y - origin.getPosition().y) + p.C * (end.getPosition().z - origin.getPosition().z));
        if (para != 0)
        {
            t = (p.D - p.A * origin.getPosition().x - p.B * origin.getPosition().y - p.C * origin.getPosition().z) / para;
        }
        else
        {
            return new Vector3(9999, 9999, 9999); ;
        }
        if (t < 1 && t > 0)
        {
            return t * (end.getPosition() - origin.getPosition()) + origin.getPosition();
        }
        else
        {
            return new Vector3(9999, 9999, 9999); ;
        }
    }

    // Returns the normalized director vector of the edge
    public Vector3 getNormalizedVector()
    {
        Vector3 v = end.getPosition() - origin.getPosition();
        return v.normalized;
    }

    // Returns the lenght of the edge
    public float getLength()
    {
        return Vector3.Distance(origin.getPosition(), end.getPosition());
    }

    public void cleanFaces(List<Face> f)
    {
        for(int i = 0; i<faces.Count; i++)
        {
            if (!f.Contains(faces[i]))
            {
                faces.RemoveAt(i);
                i=i-1;
            }
        }
    }

    override
    public string ToString()
    {
        string s = id +": ";
        s += origin.id;
        s += ' ';
        s += end.id;

        s += "    Faces: ";
        for(int i = 0; i < faces.Count; i++)
        {
            s += faces[i].id;
            s += ' ';
        }
        return s;
    }
}