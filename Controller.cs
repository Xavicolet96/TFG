using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Controller : MonoBehaviour
{

    public DataStructure data;
    public bool trigger;
    public Vertex v1, v2;

    private FSM<Controller> fsm;
    FSM<Controller> stateMachine;

    public List<Face> faces;
    public List<Vertex> vertices;
    public List<Edge> edges;

    public List<GameObject> plot;


    public void Awake()
    {
        Debug.Log("Controller awoken");
        stateMachine = new FSM<Controller>(this);

        stateMachine.CurrentState = BaseState.Instance;
        v1 = null;
        v2 = null;
        List<GameObject> plot = new List<GameObject>();
}

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Controller started");
        data = new DataStructure();
        plotStructure(data);
        

    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }



    public FSM<Controller> GetFSM()
    {
        return stateMachine;
    }

    public void destroyStructure()
    {
        for (int i = 0; i<plot.Count; i++)
        {
            Destroy(plot[i]);
        }

    }



    public void plotStructure(DataStructure data) {

        vertices = data.getVertices();
        edges = data.getEdges();
        faces = data.getFaces();
        

        for (int i = 0; i < vertices.Count; i++)
        {

            createVertexObject(vertices[i]);
        }

        for (int i = 0; i < edges.Count; i++)
        {
            createEdgeObject(edges[i]);
        }

        for (int i = 0; i < faces.Count; i++)
        {
            createFaceObject(faces[i]);
        }
    }

 

    public void createVertexObject(Vertex v)
    {
        GameObject vertex = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        vertex.AddComponent<VertexObject>();
        vertex.transform.Translate(v.position);
        vertex.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        vertex.GetComponent<VertexObject>().v = v;
        vertex.GetComponent<VertexObject>().c = this;

        plot.Add(vertex);
    }

    public bool hasVertex1()
    {
        if (v1 != null)
        {
            return true;
        }
        else return false;
    }
    public bool hasVertex2()
    {
        if (v2 != null)
        {
            return true;
        }
        else return false;
    }

    public void clearData()
    {
        v1 = null;
        v2 = null;
    }

    public bool addV(Vertex v)
    {
        if (!hasVertex1()) { 
            v1 = v;
            Debug.Log("v1 added");
            return true;
        }
        else if (!hasVertex2())
        {
            v2 = v;
            Debug.Log("v2 added");
            return true;
        }
        else
        {
            return false;
        }

    }

    public void createEdgeObject(Edge e)
    {
        Vector3 v1 = e.origin.position;
        Vector3 v2 = e.end.position;
        GameObject edge = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        edge.AddComponent<EdgeObject>();
        Vector3 vec = (v1 - v2);

        float l = vec.magnitude;
        edge.transform.localScale = new Vector3(0.05f, l / 2, 0.05f);

        Vector3 midpoint = (v1 + v2) / 2;
        edge.transform.Translate(midpoint);

        vec = vec.normalized;

        Quaternion.FromToRotation((new Vector3(0, 1, 0)), vec);

        edge.transform.rotation = Quaternion.FromToRotation((new Vector3(0, 1, 0)), vec);

        plot.Add(edge);

    }

    public void createFaceObject(Face f)
    {
        List<Vertex> vertices = f.getVertices();
        Mesh mesh = createMesh(vertices);

        GameObject face = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
        face.AddComponent<FaceObject>();

        face.GetComponent<MeshFilter>().mesh = mesh;
        //face.GetComponent<MeshRenderer>().mesh = mesh;

        plot.Add(face);

    }

    private Mesh createMesh( List<Vertex> v)
    {
        Mesh mesh = new Mesh();
        int n = v.Count;
        Vector3[] vertices = new Vector3[n];

        for (int i = 0; i < n; i++)
        {
           
            vertices[i] = v[i].position;
        }

        Vector2[] UV = new Vector2[4];
        int[] triangles = new int[(n - 2) * 3];

        UV[0] = new Vector2(-1, 1);
        UV[1] = new Vector2(1, 1);
        UV[3] = new Vector2(-1, -1);
        UV[2] = new Vector2(1, -1);


        for (int i = 0; i < n - 2; i++)
        {
            triangles[i] = 0;
            triangles[i + 1] = i + 1;
            triangles[i + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.uv = UV;
        mesh.triangles = triangles;

        return mesh;
    }
}