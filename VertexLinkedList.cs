using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class holds a list of stacks that represent the vertices and their history
public class VertexLinkedList
{
    List<Stack<Vertex>> vll = new List<Stack<Vertex>>();

    public VertexLinkedList()
    {
    }

    public void addVertex(Vertex v)
    {
        Stack<Vertex> list = new Stack<Vertex>();
        list.Push(v);
        vll.Add(list);

    }

    public List<Vertex> getVertices()
    {
        List<Vertex> activeVertices = new List<Vertex>();
        for (int i = 0; i < vll.Count; i++)
        {
            activeVertices.Add(vll[i].Peek());
        }
        return activeVertices;
    }

    public void pushVertex(Vertex previous, Vertex actual)
    {
        for (int i = 0; i < vll.Count; i++)
        {
            if (previous == vll[i].Peek())
            {
                vll[i].Push(actual);
            }
        }
    }

    public Vertex getPrevious(Vertex v)
    {
        Vertex previous, poped;
        for (int i = 0; i < vll.Count; i++)
        {
            if (v == vll[i].Peek())
            {
                poped = vll[i].Pop();
                previous = vll[i].Peek();
                return previous;
            }
        }
        return null;
    }

}

