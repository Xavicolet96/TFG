using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeObject : MonoBehaviour
{

    public GameObject edge;

    public EdgeObject(Vector3 v1, Vector3 v2)
    {
        edge = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Vector3 vec = (v1 - v2);

        float l = vec.magnitude;
        edge.transform.localScale = new Vector3(0.05f, l/2, 0.05f);

        Vector3 midpoint = (v1 + v2) / 2;
        edge.transform.Translate(midpoint);

        vec = vec.normalized;

        //Quaternion.FromToRotation((0,1,0) to vec)

        float rotz = Vector3.Angle(new Vector3(1, 0, 0), new Vector3(vec.x, vec.y, 0));
        float roty = Vector3.Angle(new Vector3(0, 0, 1), new Vector3(vec.x, 0, vec.z));

        edge.transform.Rotate(0,rotz, roty);

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
 
        Debug.Log("I've been touched (edge)");
        
    }
}
