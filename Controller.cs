using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{

    public DataStructure data;
    public bool trigger;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Controller started");
        trigger = false;
        data = new DataStructure();
        plotStructure(data, trigger);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (trigger) {
            Debug.Log("I've been triggered");
            trigger = false;
        }
    }


    public void plotStructure(DataStructure data, bool trigger) {

        List<Vertex> vertices = data.getVertices();
        List<Edge> edges = data.getEdges();
        List<Face> faces = data.getFaces();

        for (int i = 0; i < vertices.Capacity; i++)
        {
            createVertexObject(vertices[i]);
        }

        for (int i = 0; i < edges.Capacity; i++)
        {
            createEdgeObject(edges[i]);
        }

        for (int i = 0; i < faces.Capacity; i++)
        {
            createFaceObject(faces[i]);
        }
    }

    public void createVertexObject(Vertex v)
    {


    }

    public void createEdgeObject(Edge e)
    {


    }

    public void createFaceObject(Face f)
    {


    }

}





//Vector3[] Vertices = new Vector3[4];
//Vector2[] UV = new Vector2[4];
//int[] Triangles = new int[6];

//Vertices[0] = new Vector3(-1, 1);
//Vertices[1] = new Vector3(1, 1);
//Vertices[3] = new Vector3(-1,-1);
//Vertices[2] = new Vector3(1, -1);

//UV[0] = new Vector2(-1, 1);
//UV[1] = new Vector2(1, 1);
//UV[3] = new Vector2(-1, -1);
//UV[2] = new Vector2(1, -1);

//Triangles[0] = 0;
//Triangles[1] = 1;
//Triangles[2] = 3;
//Triangles[3] = 3;
//Triangles[4] = 1;
//Triangles[5] = 2;

//FaceObject face = new FaceObject(4, Vertices, UV, Triangles);

//Debug.Log("Mesh created");


//EdgeObject edge1 = new EdgeObject(Vertices[0], Vertices[1]);
//EdgeObject edge2 = new EdgeObject(Vertices[1], Vertices[2]);
//EdgeObject edge3 = new EdgeObject(Vertices[2], Vertices[3]);
//EdgeObject edge4 = new EdgeObject(Vertices[3], Vertices[0]);

//VertexObject vertex1 = new VertexObject(Vertices[0]);
//VertexObject vertex2 = new VertexObject(Vertices[1]);
//VertexObject vertex3 = new VertexObject(Vertices[2]);
//VertexObject vertex4 = new VertexObject(Vertices[3]);


//Vector3 v1 = new Vector3(1, 3, 0);

//Vector3 v2 = new Vector3(2, 4, 0);

//VertexObject vertexa = new VertexObject(v1);
//VertexObject vertexb = new VertexObject(v2);
//EdgeObject edge5 = new EdgeObject(v1, v2);


//VertexObject vertex5 = new VertexObject(new Vector3(1, 3, 0));
