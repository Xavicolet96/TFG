using System;
using UnityEngine;

// This class represents a vertex of the model
[Serializable()]
public class Vertex: Element
{
    public Vector3 position;

    // The position a step are saved
    public Vertex(Vector3 pos, int s, int id) : base(s, id)
    {
        position = pos;
    }
}