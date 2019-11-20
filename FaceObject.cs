using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObject : MonoBehaviour
{

    public FaceObject(int n, Vector3[] Vertices, Vector2[] UV, int[] Triangles) {

        //Mesh mesh = createMesh(n, Vertices);

        //GameObject face = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));

        //face.GetComponent<MeshFilter>().mesh = mesh;

      
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
        Debug.Log("I've been touched (face)");
    }


    //private Mesh createMesh(int n, Vector3[] vertices)
    //{
    //    Mesh mesh = new Mesh();

    //    Vector2[] UV = new Vector2[4];
    //    int[] triangles = new int[(n-2)*3];

    //    UV[0] = new Vector2(-1, 1);
    //    UV[1] = new Vector2(1, 1);
    //    UV[3] = new Vector2(-1, -1);
    //    UV[2] = new Vector2(1, -1);


    //    for (int i = 0; i < n-2 ; i++) {
    //        triangles[i] = 0;
    //        triangles[i + 1] = i + 1;
    //        triangles[i + 2] = i + 2;
    //    }

    //    mesh.vertices = vertices;
    //    mesh.uv = UV;
    //    mesh.triangles = triangles;

    //    return mesh;
    //}
}
