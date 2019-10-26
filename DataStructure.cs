using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStructure 
{
    public FaceTree faceTree;
    public EdgeBinaryTreeList edgeBTL;
    public VertexLinkedList vertexLL;
    public int step;
   
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
}

public class EdgeBinaryTree {

    EdgeNode root;

    public EdgeBinaryTree(Edge e) {
        root = new EdgeNode(e);
    }
    public List<Edge> getLeaves()
    {
        List<Edge> leaves = new List<Edge>();

        

        return leaves;
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

}

