using System.Collections.Generic;
using UnityEngine;

/// This class contains all the model's information: vertices, edges and faces. 
public class DataStructure 
{
    // Three structures that contain faces, edges and vertices
    public FaceTree faceTree;
    public EdgeBinaryTreeList edgeBTL;
    public VertexLinkedList vertexLL;

    // Step represents the number of folds performed
    public int step, unfoldCount;
    // Epsilon is the difference from the ideal fold due to the paper's thickness
    public float epsilon = 0.1f;

    public Vector3 mountainVec = new Vector3(0, 0, 1);
   
    /// In the construction of the data structure we generate the initial square 
    /// centered in 0,0,0 and with side 2.
    /// The structures that hold the data are created and filled
    public DataStructure() {
        
        step = 0;
        unfoldCount = 0;

        faceTree = new FaceTree();
        edgeBTL = new EdgeBinaryTreeList();
        vertexLL = new VertexLinkedList();

        Vertex v1 = new Vertex(new Vector3(-1, 1, 0), step);
        Vertex v2 = new Vertex(new Vector3(1, 1, 0), step);
        Vertex v3 = new Vertex(new Vector3(1, -1, 0), step);
        Vertex v4 = new Vertex(new Vector3(-1, -1, 0), step);

        Edge e1 = new Edge(v1, v2, step);
        Edge e2 = new Edge(v2, v3, step);
        Edge e3 = new Edge(v3, v4, step);
        Edge e4 = new Edge(v4, v1, step);

        v1.addEdge(e1);
        v1.addEdge(e4);

        v2.addEdge(e1);
        v2.addEdge(e2);

        v3.addEdge(e2);
        v3.addEdge(e3);

        v4.addEdge(e3);
        v4.addEdge(e4);

        List<Vertex> vList = new List<Vertex>();
        vList.Add(v1);
        vList.Add(v2);
        vList.Add(v3);
        vList.Add(v4);

        List<Edge> eList = new List<Edge>();
        eList.Add(e1);
        eList.Add(e2);
        eList.Add(e3);
        eList.Add(e4);

        Face f = new Face(vList, step);

        vertexLL.addVertex(v1);
        vertexLL.addVertex(v2);
        vertexLL.addVertex(v3);
        vertexLL.addVertex(v4);

        edgeBTL.addEdge(e1);
        edgeBTL.addEdge(e2);
        edgeBTL.addEdge(e3);
        edgeBTL.addEdge(e4);

        faceTree.setRoot(f);
    }

    // Gets all active faces
    public List<Face> getFaces ()
    {
        return faceTree.getFaces();
    }

    // Gets all active edges
    public List<Edge> getEdges()
    {
        return edgeBTL.getEdges();
    }
    // Gets all active vertices
    public List<Vertex> getVertices()
    {
        return vertexLL.getVertices();
    }
    
    // Given 2 vertices performs a mountain fold between the two, v1 is the one being moved
    public void mountainFoldVtoV( Vertex v1, Vertex v2)
    {
        step++;
        Vector3 normal = v1.position - v2.position;
        Vector3 midpoint = (v1.position + v2.position) / 2;

        Vector3 delta = mountainVec * epsilon;

        normal = normal + delta;

        Plane p = new Plane(normal, midpoint);

        fold(p, v1);
    }

    // Folds along a symmetry plane v being the moved vertex
    public void fold(Plane p, Vertex v)
    {
        List<Vertex> aV = getAfecctedVertices(p, v);

        generateNewCreases(p);

        applySymetry(p, aV);

        unfoldCount = 0;
    }

    // Unfolds the last step
    public void unfold()
    {
        step++;
        Vertex v, vNew;
        List<Vertex> vList = getVertices();
        for (int i = 0; i< vList.Count; i++)
        {
            v = vList[i];
            if (v.step == step -1- unfoldCount) {
                vNew = v;
                vNew.position = vertexLL.getPrevious(v).position;
                vertexLL.pushVertex(v, vNew);
            }
        }
        unfoldCount++;

    }
    /// <summary>
    /// lo passes 3 vertex
    /// genera les arestes entre els 3 si no hi son
    ///  calcula baricentre
    ///  genera vertex
    ///  genera arestes a baricentre des dels 3
    ///  genera les 3 cares de la cara original
    ///  saber si es vall o muntanya
    ///  posar el plec adicional i partir aresta
    ///  moure el vertex a mitja aresta i el de la punta
    ///  la resta es queda igual
    /// </summary>
    public void rabbitEar()
    {

    }

    // Creates vertices, edges and faces and inserts them in the model's strucutre
    public void generateNewCreases(Plane p)
    {
        List <EdgeNode> activeEdgeNodes = edgeBTL.getLeaveNodes();
        List<Vertex> activeVertices = getVertices();

        List<Vertex> cutVertices = new List<Vertex>();
        /// si tallo un vertex el tinc en compte d(etermineSide =0)
        for (int i = 0; i < activeVertices.Count; i++)
        {
            if (p.determineSide(activeVertices[i])==0)
            {
                cutVertices.Add(activeVertices[i]);
            }
        }

        /// per cada aresta que es talla faig split i genero un vertex
        for (int i = 0; i< activeEdgeNodes.Count; i++)

        {
            EdgeNode parent = activeEdgeNodes[i];
            Vertex o = parent.getEdge().origin;
            Vertex e = parent.getEdge().end;


            Vector3 point = parent.getEdge().isCut(p);

            Vector3 inf = new Vector3(999999, 999999, 999999);

            if (point != inf)
            {
                Vertex v = new Vertex(point,step);
                cutVertices.Add(v);
                vertexLL.addVertex(v);

                Edge edgel = new Edge(o, v, step);
                EdgeNode ls = new EdgeNode(parent,edgel);

                Edge edger = new Edge(v, e, step);
                EdgeNode rs = new EdgeNode(parent, edger);

                v.addEdge(edgel);
                v.addEdge(edger);

                parent.setSons(ls, rs);
            }
        }

        /// per cada vertex que comparteix cara 2 a 2 genero una aresta
        for (int i = 0; i < cutVertices.Count; i++ )
        {
            for (int j = i+1; j < cutVertices.Count; j++)
            {
                
                FaceNode f = shareFace(cutVertices[i], cutVertices[j]);
                if (f != null)
                {
                    Edge e = new Edge(cutVertices[i], cutVertices[j],step);
                    cutVertices[i].addEdge(e);
                    cutVertices[j].addEdge(e);
              
                    edgeBTL.addEdge(e);

                    /// genero cares
                    /// agafo tots els vertex de la cara original i elsdaquesta aresta nova
                    /// filtro repetits
                    ///si estan a laresta van als dos
                    ///s determineside de tots 
                    ///0 als 2
                    ///1 a f1 
                    ///-1 f2
                    ///
                    List<Vertex> l = f.getFace().getVertices();

                    if (!l.Contains(cutVertices[i]))
                    {
                        l.Add(cutVertices[i]);
                    }
                    if (!l.Contains(cutVertices[j]))
                    {
                        l.Add(cutVertices[j]);
                    }
                    int side;
                    List<Vertex> l1= new List<Vertex>();
                    List<Vertex> l2 = new List<Vertex>();

                    for (int k = 0; k < l.Count; k++)
                    {
                        side = p.determineSide(l[k]);
                        if(side == 1)
                        {
                            l1.Add(l[k]);
                        }
                        else if (side == -1)
                        {
                            l2.Add(l[k]);
                        }
                        else
                        {
                            l1.Add(l[k]);
                            l2.Add(l[k]);
                        }
                    }

                    Face f1 = new Face(l1,step);
                    Face f2 = new Face(l2,step);

                    FaceNode fn1 = new FaceNode(f,f1);
                    FaceNode fn2 = new FaceNode(f, f2);

                    f.addSon(fn1);
                    f.addSon(fn2);
                }
                
            }
        }        
    }

    // Applies a specular symmetry across the plain to each vertex and pushes it into the structure
    public void applySymetry(Plane p, List<Vertex> vL)
    {
        Matrix4x4 C,S,Cinv,M;
        Vector4 v1 = new Vector4(p.n.x, p.n.y, p.n.z,0);
        Vector4 v2 = new Vector4(p.vd1.x, p.vd1.y, p.vd1.z, 0);
        Vector4 v3 = new Vector4(p.vd2.x, p.vd2.y, p.vd2.z, 0);
        Vector3 planep = p.getPoint();
        Vector4 v4 = new Vector4(planep.x, planep.y, planep.z, 1);
        C = new Matrix4x4(v1, v2, v3, v4);
        Cinv = C.inverse;
        S = Matrix4x4.identity;
        S.m00 = -1;

        Vector4 q;

        M = C * S * Cinv;

        for (int i = 0; i < vL.Count; i++)
        {
            q= new Vector4(vL[i].position.x, vL[i].position.y, vL[i].position.z, 1);
            Vector4 point = M.MultiplyPoint(q);
            Vertex newV = new Vertex(new Vector3(point.x, point.y, point.z), step);
            newV.edges = vL[i].edges;
            vertexLL.pushVertex(vL[i], newV);
        }
    }

    // Returns wether the 2 vertices share a face and returns it if it exists
    public FaceNode shareFace(Vertex v1, Vertex v2)
    {
        FaceNode f = null;
        List<FaceNode> faceNodes = faceTree.getFaceNodes();
        for (int i = 0; i< faceNodes.Count; i++)
        {
            f = faceNodes[i];
            if (f.getFace().has(v1) && f.getFace().has(v2))
            {
                return f;
            }
        }
        return f;
    }

    // Returns all the vertices on one side of the plane (v's side)
    public List<Vertex> getAfecctedVertices(Plane p, Vertex v)
    {
        List <Vertex> vertices = getVertices();
        int side = p.determineSide(v);
        List<Vertex> affectedV = new List<Vertex>();

        for (int i = 0; i< vertices.Count; i++)
        {
            if (p.determineSide(vertices[i]) == side) {
                affectedV.Add(vertices[i]);
            }
        }
        return affectedV;
    }
}