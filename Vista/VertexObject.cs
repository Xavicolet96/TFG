using UnityEngine;

// This class encodes the behaviour of the gameObject that represents a face
public class VertexObject : ElementObject
{
    public VertexNode v;

    public VertexObject()
    {      
    }

    // Activated when it is touchedS
    private void OnMouseDown()
    {
        c.selecting = true;
        c.addV(v);
        select();
    }
    public void OnMouseOver()
    {
        Debug.Log(v);
    }
}