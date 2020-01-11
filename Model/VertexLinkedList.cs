using System;
using System.Collections.Generic;

// This class holds a list of stacks that represent the vertices and their history
[Serializable()]
public class VertexLinkedList
{
    public List<VertexNode> vll;
    public int count;

    // Initializes the list
    public VertexLinkedList()
    {
        vll = new List<VertexNode>();
        count = 0;
    }

    // Adds a vertex to the list
    public void addVertex(VertexNode vn)
    {
        vll.Add(vn);
        count++;
    }

    // Returns all active vertices
    public List<VertexNode> getVertices()
    {
        List<VertexNode> activeVertices = new List<VertexNode>();
        for (int i = 0; i < vll.Count; i++)
        {
            activeVertices.Add(vll[i]);
        }
        return activeVertices;
    }

    // Returns all active vertices in step n
    public List<VertexNode> getVertices(int n)
    {
        List<VertexNode> activeVertices = new List<VertexNode>();
        for (int i = 0; i < vll.Count; i++)
        {
            if (vll[i].vertices[0].step <= n)
            {
                int j = 0;
                int m = 0;
                while(j< vll[i].vertices.Count)
                {
                    if ( vll[i].vertices[j].step <= n)
                    {
                        m = j;
                    }
                    j++;
                }

                VertexNode vn = new VertexNode(vll[i].vertices[m]);
                activeVertices.Add(vn);
            }

                
            
        }
        return activeVertices;
    }
}