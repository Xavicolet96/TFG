using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents a vertex of the model
public class Vertex: Element
{
    public Vector3 position;
    public List<Edge> edges = new List<Edge>();

    public Vertex(Vector3 pos, int s): base(s)
    {
        position = pos;
    }


    public void addEdge(Edge e)
    {
        edges.Add(e);
    }

    public void setPosition(Vector3 pos)
    {
        position = pos;
    }

    public List<Edge> getEdges()
    {
        return edges;
    }

    public Vector3 getPosition()
    {
        return position;
    }
}

