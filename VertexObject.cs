using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexObject : MonoBehaviour
{
    public Camera cam;
    public VertexObject(Vector3 coor)
    {
        GameObject vertex = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        vertex.transform.Translate(coor);
        vertex.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {


                Debug.Log(hit.point);
             }
        }
        
    }

    private void OnMouseDown()
    {
        Debug.Log("I've been touched (vertex)");
    }
}
