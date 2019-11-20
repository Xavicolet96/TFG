using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexObject : MonoBehaviour
{
    public Camera cam;
    public Controller c;
    public Vertex v;
    public VertexObject(Vector3 coor)
    {
        //GameObject vertex = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //vertex.transform.Translate(coor);
        //vertex.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("I've been touched (vertex)");
        c.addV(v);
    }
}
