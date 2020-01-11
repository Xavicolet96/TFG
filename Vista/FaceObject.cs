using UnityEngine;

// This class encodes the behaviour of the gameObject that represents a face
public class FaceObject : ElementObject
{
    public FaceObject()
    {
    }

    // Activated when it is touched
    private void OnMouseDown()
    {
        Debug.Log("I've been touched (face)");
    }

    override
    public void deselect()
    {

    }
}