using System.Collections.Generic;
using UnityEngine;

public class DataStructure 
{
    public FaceTree faceTree;
    public EdgeBinaryTreeList edgeBTL;
    public VertexLinkedList vertexLL;

    public int step;
    public float epsilon = 0.05f;
    public Vector3 mountainVec = new Vector3(0, 0, -1);
   
    public DataStructure() {
        
        step = 0;

        faceTree = new FaceTree();
        edgeBTL = new EdgeBinaryTreeList();
        vertexLL = new VertexLinkedList();

        Vertex v1 = new Vertex(new Vector3(-1, 1, 0));
        Vertex v2 = new Vertex(new Vector3(1, 1, 0));
        Vertex v3 = new Vertex(new Vector3(1, -1, 0));
        Vertex v4 = new Vertex(new Vector3(-1, -1, 0));

        Edge e1 = new Edge(v1, v2);
        Edge e2 = new Edge(v2, v3);
        Edge e3 = new Edge(v3, v4);
        Edge e4 = new Edge(v4, v1);

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

        Face f = new Face(vList, eList);

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

    public List<Face> getFaces ()
    {
        return faceTree.getFaces();
    }

    public List<Edge> getEdges()
    {
        return edgeBTL.getEdges();
    }

    public List<Vertex> getVertices()
    {
        return vertexLL.getVertices();
    }

    public void mountainFoldVtoV( Vertex v1, Vertex v2)
    {
        Vector3 normal = v1.position - v2.position;
        Vector3 midpoint = (v1.position + v2.position) / 2;

        Vector3 delta = mountainVec * epsilon;

        normal = normal + delta;

        Plane p = new Plane(normal, midpoint);

        getAfecctedVertices(p, v2);

        generateNewCreases(p);

    }
    public void generateNewCreases(Plane p)
    {
        List <EdgeNode> activeEdgeNodes = edgeBTL.getLeaveNodes();
        List<Vertex> activeVertices = getVertices();

        List<Vertex> cutVertices = new List<Vertex>();
        /// si tallo un vertex el tinc en compte d(etermineSide =0)
        for (int i = 0; i < activeVertices.Capacity; i++)
        {
            if (p.determineSide(activeVertices[i])==0)
            {
                cutVertices.Add(activeVertices[i]);
            }
        }

        /// per cada aresta que es talla faig split i genero un vertex
        for (int i = 0; i< activeEdgeNodes.Capacity; i++)

        {
            EdgeNode parent = activeEdgeNodes[i];
            Vertex o = parent.getEdge().origin;
            Vertex e = parent.getEdge().end;


            Vector3 point = parent.getEdge().isCut(p);
            if (point != Vector3.negativeInfinity)
            {
                Vertex v = new Vertex(point);
                cutVertices.Add(v);
                vertexLL.addVertex(v);

                Edge edgel = new Edge(o, v);
                EdgeNode ls = new EdgeNode(parent,edgel);

                Edge edger = new Edge(v, e);
                EdgeNode rs = new EdgeNode(parent, edger);

                v.addEdge(edgel);
                v.addEdge(edger);

                
                parent.setSons(ls, rs);

            }
        }

        /// per cada vertex que comparteix cara 2 a 2 genero una aresta
        for (int i = 0; i < cutVertices.Capacity; i++ )
        {
            for (int j = 0; j < cutVertices.Capacity; j++)
            {
                if (i != j)
                {
                    FaceNode f = shareFace(cutVertices[i], cutVertices[j]);
                    if (f != null)
                    {
                        Edge e = new Edge(cutVertices[i], cutVertices[j]);
                        cutVertices[i].addEdge(e);
                        cutVertices[j].addEdge(e);
              
                        edgeBTL.addEdge(e);

                        /// genero cares

                        Face f1 = new Face();
                        Face f2 = new Face();

                        FaceNode fn1 = new FaceNode(f,f1);
                        FaceNode fn2 = new FaceNode(f, f2);

                        f.addSon(fn1);
                        f.addSon(fn2);
                    }
                }
            }
        }
        /// genero cares
        
    }

    public FaceNode shareFace(Vertex v1, Vertex v2)
    {
        FaceNode f = null;
        List<FaceNode> faceNodes = getFaceNodes();
        for (int i = 0; i< faceNodes.Capacity; i++)
        {
            f = faceNodes[i];
            if (f.getFace().has(v1) && f.getFace().has(v2))
            {
                return f;
            }
        }
        return f;
    }

    public List<Vertex> getAfecctedVertices(Plane p, Vertex v)
    {
        List <Vertex> vertices = getVertices();
        int side = p.determineSide(v);
        for (int i = 0; i< vertices.Capacity; i++)
        {
            if (p.determineSide(vertices[i]) == side) {
                vertices.Add(vertices[i]);
            }
        }
        return vertices;
    }

    public List<FaceNode> getFaceNodes()
    {
        List<FaceNode> list = new List<FaceNode>();
        list.AddRange(faceTree.getFaceNodes());
        return list;
    }
}

public class Plane {
    public float A,B,C,D;

    public Plane(Vector3 normal, Vector3 p) 
    {
        A = normal.x;
        B = normal.y;
        C = normal.z;
        D = A * p.x + B * p.y + C * p.z;
    }

    public int determineSide(Vertex v)
    {
        if (A * v.position.x + B * v.position.y + C * v.position.z == D) return 0;
        if (A * v.position.x + B * v.position.y + C * v.position.z < D) return -1;
        else return 1;

    }

}

public class FaceTree
{
    FaceNode root;

    public FaceTree() {
    }

    public void setRoot(Face f)
    {
        root = new FaceNode(f);
    }

    public List<Face> getFaces() 
    {
        List<Face> faces = new List<Face>();
        faces.AddRange(root.getLeaves());
        return faces; 
    }

    public List<FaceNode> getFaceNodes()
    {
        List<FaceNode> list = new List<FaceNode>();
        if (!root.getHasSons())
        {
            list.Add(root);
            return list;
        }
        else
        {
            list.AddRange(root.getLeaveNodes());
        }
        return list;
    }

}

public class FaceNode
{
    Face face;
    FaceNode parent;
    List<FaceNode> sons;
    bool hasSons;

    public FaceNode(Face f) 
    {
        sons = new List<FaceNode>();
        face = f;
        hasSons = false;
    }

    public FaceNode(FaceNode p, Face f)
    {
        sons = new List<FaceNode>();
        face = f;
        parent = p;
    }

    public void addSon(FaceNode fn) {
        sons.Add(fn);
        hasSons = true;
    }

    public Face getFace() {
        return face;
    }

    public FaceNode getParent() {
        return parent;
    }

    public bool getHasSons()
    {
        return hasSons;
    }

    public List<Face> getLeaves() {
        List<Face> leaves = new List<Face>();
        if (hasSons){
            for (int i = 0; i < sons.Capacity ; i++)
            {
                leaves.AddRange(sons[i].getLeaves());
            }
        }
        else
        {
            leaves.Add(face);
            return leaves;
        }
        return leaves;
    }
    public List<FaceNode> getLeaveNodes()
    {
        List<FaceNode> leaves = new List<FaceNode>();
        for (int i = 0; i<sons.Capacity;i++)
        {
            if (!sons[i].getHasSons())
            {
                leaves.Add(sons[i]);
            }
            else
            {
                sons[i].getLeaveNodes();
            }
        }
        return leaves;
    }

}

public class EdgeBinaryTreeList
{
    List<EdgeBinaryTree> edgeBTL;
    public EdgeBinaryTreeList()
    {
        edgeBTL = new List<EdgeBinaryTree>();
    }

    public void addEdge(Edge e)
    {
        EdgeBinaryTree edgeBT = new EdgeBinaryTree(e);
        edgeBTL.Add(edgeBT);
    }

    public List<Edge> getEdges()
    {
        List<Edge> list = new List<Edge>();
        for (int i = 0; i < edgeBTL.Capacity; i++)
        {
            list.AddRange(edgeBTL[i].getLeaves());
        }
        return list;
    } 

    public List<EdgeNode> getLeaveNodes()
    {
        List<EdgeNode> list = new List<EdgeNode>();
        for (int i = 0; i < edgeBTL.Capacity; i++)
        {
            list.AddRange(edgeBTL[i].getLeaveNodes());
        }
        return list;
    }
}

public class EdgeBinaryTree {

    EdgeNode root;

    public EdgeBinaryTree(Edge e) {
        root = new EdgeNode(e);
    }
    public List<Edge> getLeaves()
    {
        List<Edge> leaves = new List<Edge>();
        leaves.AddRange(root.getLeaves());
        return leaves;
    }
    public List<EdgeNode> getLeaveNodes()
    {
        List<EdgeNode> list = new List<EdgeNode>();

        if (!root.getHasSons())
        {
            list.Add(root);
            return list;
        }
        else
        {
            list.AddRange(root.getLeaveNodes());
        }
        return list;
    }
}

public class EdgeNode {

    Edge edge;
    EdgeNode parent;
    EdgeNode leftSon;
    EdgeNode rightSon;
    bool hasSons;

    public EdgeNode(Edge e) {
        edge = e;
    }

    public EdgeNode (EdgeNode p, Edge e)
    {
        parent = p;
        edge = e;
    }

    public List<EdgeNode> getSons()
    {
        List<EdgeNode> list = new List<EdgeNode>();
        list.Add(leftSon);
        list.Add(rightSon);
        return list;
    }

    public bool getHasSons()
    {
        return hasSons;
    }

    public EdgeNode getParent()
    {
        return parent;
    }

    public Edge getEdge()
    {
        return edge;
    }

    public void setSons(EdgeNode enl, EdgeNode enr) {
        leftSon = enl;
        rightSon = enr;
        hasSons = true;
    }

    public List<Edge> getLeaves()
    {
        List<Edge> leaves = new List<Edge>();
        if (hasSons)
        {
            leaves.AddRange(leftSon.getLeaves());
            leaves.AddRange(rightSon.getLeaves());
        }
        else
        {
            leaves.Add(edge);
            return leaves;
        }
        return leaves;
    }

    public List<EdgeNode> getLeaveNodes()
    {
        List<EdgeNode> leaves = new List<EdgeNode>();

        if (!leftSon.getHasSons())
        {
            leaves.Add(leftSon);
        }
        else
        {
            leftSon.getLeaveNodes();
        }

        if (!rightSon.getHasSons())
        {
            leaves.Add(rightSon);
        }
        else
        {
            rightSon.getLeaveNodes();
        }

        return leaves;
    }


}

public class VertexLinkedList
{
    List<Stack<Vertex>> vll = new List<Stack<Vertex>>();

    public VertexLinkedList()
    {
    }

    public void addVertex(Vertex v) { 
        Stack<Vertex> list = new Stack<Vertex>();
        list.Push(v);
        vll.Add(list);
        
    }

    public List<Vertex> getVertices() 
    {
        List<Vertex> activeVertices = new List<Vertex>();
        for(int i = 0; i < vll.Capacity; i++)
        {
            activeVertices.Add(vll[i].Peek());
        }
        return activeVertices;
    }

}

public class Vertex
{
    public Vector3 position;
    public List<Edge> edges = new List<Edge>();

    public Vertex(Vector3 pos)
    {
        position = pos;
    }


    public void addEdge(Edge e) {
        edges.Add(e);
    }

    public void setPosition(Vector3 pos) {
        position = pos;
    }

    public List<Edge> getEdges() {
        return edges;
    }

    public Vector3 getPosition() {
        return position;
    }
}

public class Edge
{
    public Vertex origin;
    public Vertex end;
    public Face left;
    public Face right;
    
    public Edge(Vertex o, Vertex e)
    {
        origin = o;
        end = e;
    }

    public Vector3 isCut (Plane p)
    {
        // r = t(end-origin) + origin
        // A(t(end.x-origin.x)+origin.x) +B(t(end.y-origin.y)+origin.y) +C(t(end.z-origin.z)+origin.z)  = D
        // A*t*end.x-A*t*origin.x +A*origin.x
        // t(Aend-Aorigin)...= D-Aorigin-....

        float t;
        float para = (p.A * (end.position.x - origin.position.x) + p.B * (end.position.y - origin.position.y) + p.C * (end.position.z - origin.position.z));
        if (para!= 0)
        {
            t = (p.D - p.A * origin.position.x - p.B * origin.position.y - p.C * origin.position.z) / para;
        }
        else
        {
            return Vector3.negativeInfinity;
        }

        if (t<1 && t > 0)
        {
            return t * (end.position - origin.position) + origin.position;
        }
        else
        {
            return Vector3.negativeInfinity;
        }
        //return Vector3.negativeInfinity;
    }

}

public class Face
{
    List<Vertex> vList;
    List<Edge> eList;

    public Face(List<Vertex> vl, List<Edge> el)
    {
        vList = vl;
        eList = el;
    }

    public List<Vertex>  getVertices() {
        return vList;
    }

    public bool has(Vertex v)
    {
        for (int i = 0; i < vList.Capacity; i++)
        {
            if (v == vList[i])
            {
                return true;
            }
        }
        return false;
    }

    public bool has(Edge v)
    {
        for (int i = 0; i < eList.Capacity; i++)
        {
            if (v == eList[i])
            {
                return true;
            }
        }
        return false;
    }
}