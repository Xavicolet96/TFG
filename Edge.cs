using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class represents an edge of the model
public class Edge: Element
{
    public Vertex origin;
    public Vertex end;
    public Face left;
    public Face right;

    public Edge(Vertex o, Vertex e, int s) : base(s)
    {
        origin = o;
        end = e;
    }

    public Vector3 isCut(Plane p)
    {
        // r = t(end-origin) + origin
        // A(t(end.x-origin.x)+origin.x) +B(t(end.y-origin.y)+origin.y) +C(t(end.z-origin.z)+origin.z)  = D
        // A*t*end.x-A*t*origin.x +A*origin.x
        // t(Aend-Aorigin)...= D-Aorigin-....

        float t;
        float para = (p.A * (end.position.x - origin.position.x) + p.B * (end.position.y - origin.position.y) + p.C * (end.position.z - origin.position.z));
        if (para != 0)
        {
            t = (p.D - p.A * origin.position.x - p.B * origin.position.y - p.C * origin.position.z) / para;
        }
        else
        {
            return new Vector3(999999, 999999, 999999);
        }

        if (t < 1 && t > 0)
        {
            return t * (end.position - origin.position) + origin.position;
        }
        else
        {
            return new Vector3(999999, 999999, 999999);
        }
        //return Vector3.negativeInfinity;
    }

}


