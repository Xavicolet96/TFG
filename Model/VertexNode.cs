using System;
using System.Collections.Generic;
using UnityEngine;

// This class saves a list of edges connected to it and a list of vertices (all the positions of the same vertex)
[Serializable()]
public class VertexNode 
{
    public List<Edge> edges;
    public List<Vertex> vertices;
    public int id;

    // Initializes the lists and adds the first vertex
    public VertexNode (Vertex v)
    {
        edges = new List<Edge>();
        vertices = new List<Vertex>();
        vertices.Add(v);
        id = v.id;
    }

    // Adds an edge to the list
    public void addEdge(Edge e)
    {
        edges.Add(e);
    }

    // Returns the last added vertex
    public Vertex peek()
    {
        return vertices[vertices.Count - 1];
    }

    // Returns the n-last added vertex
    public Vertex peek(int i)
    {
        return vertices[vertices.Count - i];
    }

    // Pushes a new vertex
    public void Add(Vertex v)
    {
        vertices.Add(v);
    }

    // Returns the current position of the vertex
    public Vector3 getPosition()
    {
        return peek().position;
    }

    override
    public string ToString()
    {
        string s = "";
        s +=  vertices[0].id;
        s += ' ';
        s += getPosition();

        s += "   Edges: ";
        for (int i = 0; i < edges.Count; i++)
        {
            s += edges[i].id;
            s += ' ';
        }
        return s;
    }
}