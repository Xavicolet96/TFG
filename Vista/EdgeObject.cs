using System.Threading;
using UnityEngine;

// This class encodes the behaviour of the gameObject that represents a edge
public class EdgeObject : ElementObject
{
    public Edge e;

    public EdgeObject()
    {
    }

    // Activated when it is touched
    private void OnMouseDown()
    {
        c.selecting = true;
        c.addE(e);
        select();
    }

    public void OnMouseOver()
    {
        //Thread.Sleep(600);
        Debug.Log(e);
    }
}