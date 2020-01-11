 using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Xml.Serialization;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public DataStructure data;
    public VertexNode v1, v2;
    public Edge e1, e2;
    public Vector3 p1,p2;
    public GameObject po1, po2;
    public List<Face> faces;
    public List<VertexNode> vertices;
    public List<Edge> edges;

    private FSM<Controller> stateMachine;

    public List<ElementObject> plot;
    public Camera cam;

    public Boolean selecting;
    public string filePath;

    // Initializes all the variables
    public void Awake()
    {
        stateMachine = new FSM<Controller>(this);
        stateMachine.CurrentState = BaseState.Instance;
        v1 = null;
        v2 = null;
        e1 = null;
        e2 = null;
        p1 = new Vector3(9999, 9999, 9999);
        p2 = new Vector3(9999, 9999, 9999);
        selecting = false;
        filePath = "model.json";

        plot = new List<ElementObject>();
    }

    // Creates and plots the model, it also gets the model to camera vector
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        Vector3 v = -cam.transform.position;
        v.Normalize();

        data = new DataStructure(v);
        plotStructure(data);
        
    }

    // Updates the state machine and checks for clicks
    void Update()
    {
        stateMachine.Update();
        if (Input.GetMouseButtonDown(0) && !selecting)
        {
            Ray r = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 v = r.origin;
            v.z = 0;
            v = v * 10;
            addP(v);
        }
        selecting = false;
     }

    // Returns the state machine
    public FSM<Controller> GetFSM()
    {
        return stateMachine;
    }

    // Creates vertices, edges and faces that are active in the model and stores it in plot
    public void plotStructure(DataStructure data)
    {
        Debug.Log(data.ToString());
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
            createFaceObject2(faces[i]);
        }
    }

    // Creates vertices, edges and faces that are active in the model in step n and stores it in plot
    public void plotStructure(DataStructure data, int n)
    {
        vertices = data.getVertices(n);
        Debug.Log("V: "+vertices.Count);
        edges = data.getEdges(n);
        Debug.Log("E: " + edges.Count);
        faces = data.getFaces(n);
        Debug.Log("F: " + faces.Count);

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
            createFaceObject2(faces[i]);
        }
    }

    // Destroys the gameObjects in plot
    public void destroyStructure()
    {
        for (int i = 0; i < plot.Count; i++)
        {
            Destroy(plot[i].gameObject);

        }
        plot = new List<ElementObject>();
    }

    // Creates the gameObjects necessary to visualize the model
    public void createVertexObject(VertexNode v)
    {
        GameObject vertex = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        vertex.AddComponent<VertexObject>();
        vertex.transform.Translate(v.getPosition());
        vertex.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
        vertex.GetComponent<VertexObject>().v = v;
        vertex.GetComponent<VertexObject>().c = this;

        plot.Add(vertex.GetComponent<VertexObject>());
    }
    public void createEdgeObject(Edge e)
    {
        Vector3 v1 = e.origin.getPosition();
        Vector3 v2 = e.end.getPosition();
        GameObject edge = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        edge.AddComponent<EdgeObject>();
        edge.GetComponent<EdgeObject>().e = e;
        edge.GetComponent<EdgeObject>().c = this;
        Vector3 vec = (v1 - v2);

        float l = vec.magnitude;
        edge.transform.localScale = new Vector3(0.02f, l / 2, 0.02f);

        Vector3 midpoint = (v1 + v2) / 2;
        edge.transform.Translate(midpoint);

        vec = vec.normalized;

        Quaternion.FromToRotation((new Vector3(0, 1, 0)), vec);

        edge.transform.rotation = Quaternion.FromToRotation((new Vector3(0, 1, 0)), vec);

        plot.Add(edge.AddComponent<EdgeObject>());

    }
    public void createFaceObject2(Face f)
    {
        List<VertexNode> verticesN = f.vList;
        List<Vertex> vertices = new List<Vertex>(); ;
        for (int i = 0; i < verticesN.Count; i++)
        {
            vertices.Add(verticesN[i].peek());
        }

        GameObject[] myObject = new GameObject[2];

        for (int x = 0; x < 2; x++)
        {
            //New mesh and game object
            myObject[x] = new GameObject();
            Mesh mesh = new Mesh();

            //Components
            MeshFilter MF = myObject[x].AddComponent<MeshFilter>();
            MeshRenderer MR = myObject[x].AddComponent<MeshRenderer>();
            myObject[x].AddComponent<FaceObject>();
            //myObject[x].AddComponent();

            //Create mesh
            mesh = CreateMesh2(x, vertices);

            //if (x == 0)
            //{
                Material m = Resources.Load<Material>("Mat");
                myObject[x].GetComponent<MeshRenderer>().material = m;
            //}

            //else
            //{
                
            //    Material m = Resources.Load<Material>("Mat2");
            //    myObject[x].GetComponent<MeshRenderer>().material = m;
            //}


            //Assign mesh to game object
            MF.mesh = mesh;
            plot.Add(myObject[x].GetComponent<FaceObject>());
        }
    }
    public Mesh CreateMesh2(int n, List<Vertex>nodePositions)
    {

        //Create a new mesh
        Mesh mesh = new Mesh();

        //Vertices
        var vertex = new Vector3[nodePositions.Count];

        for (int x = 0; x < nodePositions.Count; x++)
        {
            vertex[x] = nodePositions[x].position;
        }

        //UVs
        var uvs = new Vector2[vertex.Length];

        for (int x = 0; x < vertex.Length; x++)
        {
            if ((x % 2) == 0)
            {
                uvs[x] =new  Vector2(0, 0);
            }
            else
            {
                uvs[x] = new Vector2(1, 1);
            }
        }

        //Triangles
        var tris = new int[3 * (vertex.Length - 2)];    //3 verts per triangle * num triangles
        int C1, C2, C3;


        if (n == 0)
        {
            C1 = 0;
            C2 = 1;
            C3 = 2;

            for (int x = 0; x < tris.Length; x += 3)
            {
                tris[x] = C1;
                tris[x + 1] = C2;
                tris[x + 2] = C3;

                C2++;
                C3++;
            }
        }
        else
        {
            C1 = 0;
            C2 = vertex.Length - 1;
            C3 = vertex.Length - 2;

            for (int x = 0; x < tris.Length; x += 3)
            {
                tris[x] = C1;
                tris[x + 1] = C2;
                tris[x + 2] = C3;

                C2--;
                C3--;
            }
        }

        //Assign data to mesh
        mesh.vertices = vertex;
        mesh.uv = uvs;
        mesh.triangles = tris;

        //Recalculations
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();

        //Return the mesh
        return mesh;
    }

    public GameObject createPointObject(Vector3 v)
    {
        GameObject po = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        po.AddComponent<PointObject>();
        po.transform.Translate(v);
        po.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        //po.GetComponent<PointObject>().c = this;
        po.GetComponent<PointObject>().select();

        return po;
    }

    // Checks if an element has been selected thus saved
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
    public bool hasPoint1()
    {
        if (p1 != new Vector3(9999, 9999, 9999))
        {
            return true;
        }
        else return false;
    }
    public bool hasPoint2()
    {
        if (p2 != new Vector3(9999, 9999, 9999))
        {
            return true;
        }
        else return false;
    }
    public bool hasEdge1()
    {
        if (e1 != null)
        {
            return true;
        }
        else return false;
    }
    public bool hasEdge2()
    {
        if (e2 != null)
        {
            return true;
        }
        else return false;
    }

    // Saves an element in an empty spot
    public bool addV(VertexNode v)
    {
        if (!hasVertex1())
        {
            v1 = v;
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
    public bool addP(Vector3 v)
    {
        if (!hasPoint1())
        {
            p1 = v;
            Debug.Log("p1 added");
            po1 = createPointObject(v);
            return true;
        }
        else if (!hasPoint2())
        {
            p2 = v;
            Debug.Log("p2 added");
            po2 = createPointObject(v);
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool addE(Edge e)
    {
        if (!hasEdge1())
        {
            e1 = e;
            Debug.Log("e1 added");
            return true;
        }
        else if (!hasEdge2())
        {
            e2 = e;
            Debug.Log("e2 added");
            return true;
        }
        else
        {
            return false;
        }
    }

    // Clears all the elements that have been selected
    public void clearData()
    {
        v1 = null;
        v2 = null;
        e1 = null;
        e2 = null;
        p1 = new Vector3(9999, 9999, 9999);
        p2 = new Vector3(9999, 9999, 9999);
        Destroy(po1);
        Destroy(po2);
    }

    public void deselect()
    {
        for (int i = 0;i<plot.Count; i++)
        {
            plot[i].deselect();
        }
        p1 = new Vector3(9999, 9999, 9999);
        p2 = new Vector3(9999, 9999, 9999);
        Destroy(po1);
        Destroy(po2);
    }

    public void save()
    {
        //FileStream f = new FileStream("model.json", FileMode.Create, FileAccess.Write);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);

        Debug.Log("Saved");
    }

    public void load()
    {
        // If the file doesn't exist then just return the default object.
        if (!File.Exists(filePath))
        {
            Debug.LogErrorFormat("ReadFromFile({0}) -- file not found", filePath);
        }
        else
        {
            // If the file does exist then read the entire file to a string.
            string contents = File.ReadAllText(filePath);

            // Otherwise we can just use JsonUtility to convert the string to a new SaveData object.
            data = JsonUtility.FromJson<DataStructure>(contents);
        }
    }

    public void BButton()
    {
        GetFSM().ChangeState(SelectedVVM.Instance);
    }
    public void MButton()
    {
        if (GetFSM().CurrentState == SelectedVV.Instance)
        {
            GetFSM().ChangeState(SelectedVVM.Instance);
        }
        if (GetFSM().CurrentState == SelectedEE.Instance)
        {
            GetFSM().ChangeState(SelectedEEM.Instance);
        }
        if (GetFSM().CurrentState == SelectedVP.Instance)
        {
            GetFSM().ChangeState(SelectedVPM.Instance);
        }

    }
    public void WButton()
    {
        if (GetFSM().CurrentState == SelectedVV.Instance)
        {
            GetFSM().ChangeState(SelectedVVW.Instance);
        }
        if (GetFSM().CurrentState == SelectedEE.Instance)
        {
            GetFSM().ChangeState(SelectedEEW.Instance);
        }

    }
    public void CButton()
    {
        if (GetFSM().CurrentState == SelectedVV.Instance)
        {
            GetFSM().ChangeState(SelectedVVC.Instance);
        }
        if (GetFSM().CurrentState == SelectedVP.Instance)
        {
            GetFSM().ChangeState(SelectedVPC.Instance);
        }

    }
    public void OButton()
    {
        if (GetFSM().CurrentState == SelectedVV.Instance)
        {
            GetFSM().ChangeState(SelectedVVM.Instance);
        }

    }
    public void PButton()
    {
        if (GetFSM().CurrentState == SelectedVP.Instance)
        {
            GetFSM().ChangeState(SelectedVPP.Instance);
        }

    }
    public void LButton()
    {
        if (GetFSM().CurrentState == BaseState.Instance)
        {
            GetFSM().ChangeState(SelectedL.Instance);
        }

    }
    public void SButton()
    {
        if (GetFSM().CurrentState == BaseState.Instance)
        {
            GetFSM().ChangeState(SelectedS.Instance);
        }

    }
    public void UButton()
    {
        if (GetFSM().CurrentState == BaseState.Instance)
        {
            GetFSM().ChangeState(SelectedU.Instance);
        }
    }
    public void YButton()
    {
        if (GetFSM().CurrentState == BaseState.Instance)
        {
            GetFSM().ChangeState(SelectedY.Instance);
        }
    }

    public void QButton()
    {
        if (GetFSM().CurrentState == BaseState.Instance)
        {
            GetFSM().ChangeState(SelectedQ.Instance);
        }
    }

    // Plots all the steps until the present one in sequence
    public void play()
    {
        destroyStructure();
        for (int i = 0; i< data.step; i++)
        {
            destroyStructure();
            plotStructure(data, i);
            Thread.Sleep(2000);
        }
        plotStructure(data);
    }

    public void resetModel()
    {
       
        Vector3 v = -cam.transform.position;
        v.Normalize();
        data = new DataStructure(v);
        plotStructure(data);
    }
}