using System;
using System.Collections.Generic;
using UnityEngine;

/// This class contains all the model's information: vertices, edges and faces. 
[Serializable()]
public class DataStructure  
{
    // Three structures that contain faces, edges and vertices
    public VertexLinkedList vertexLL;
    public EdgeBinaryTreeList edgeBTL;
    public FaceTree faceTree;

    // Step represents the number of folds performed
    public int step, unfoldCount;
    // Epsilon is the difference from the ideal fold due to the paper's thickness
    public float epsilon = 0.1f;
    public Vector3 camToModel;

    // In the construction of the data structure we generate the initial square centered in 0,0,0 and with side 2. The structures that hold the data are created and filled
    public DataStructure(Vector3 v)
    {
        camToModel = v;
        step = 0;
        unfoldCount = 0;

        faceTree = new FaceTree();
        edgeBTL = new EdgeBinaryTreeList();
        vertexLL = new VertexLinkedList();

        Vertex v1 = new Vertex(new Vector3(-1, 1, 0), step,0);
        Vertex v2 = new Vertex(new Vector3(1, 1, 0), step,1);
        Vertex v3 = new Vertex(new Vector3(1, -1, 0), step,2);
        Vertex v4 = new Vertex(new Vector3(-1, -1, 0), step,3);

        VertexNode vn1 = new VertexNode(v1);
        VertexNode vn2 = new VertexNode(v2);
        VertexNode vn3 = new VertexNode(v3);
        VertexNode vn4 = new VertexNode(v4);

        Edge e1 = new Edge(vn1, vn2, step,0);
        Edge e2 = new Edge(vn2, vn3, step,1);
        Edge e3 = new Edge(vn3, vn4, step,2);
        Edge e4 = new Edge(vn4, vn1, step,3);


        vn1.addEdge(e1);
        vn1.addEdge(e4);

        vn2.addEdge(e1);
        vn2.addEdge(e2);

        vn3.addEdge(e2);
        vn3.addEdge(e3);

        vn4.addEdge(e3);
        vn4.addEdge(e4);

        List<VertexNode> vList = new List<VertexNode>();
        vList.Add(vn1);
        vList.Add(vn2);
        vList.Add(vn3);
        vList.Add(vn4);

        List<Edge> eList = new List<Edge>();
        eList.Add(e1);
        eList.Add(e2);
        eList.Add(e3);
        eList.Add(e4);

        Face f = new Face(vList, step,0);

        for (int i = 0; i < 4; i++)
        {
            eList[i].faces.Add(f);
        }

        vertexLL.addVertex(vn1);
        vertexLL.addVertex(vn2);
        vertexLL.addVertex(vn3);
        vertexLL.addVertex(vn4);

        edgeBTL.addEdge(e1);
        edgeBTL.addEdge(e2);
        edgeBTL.addEdge(e3);
        edgeBTL.addEdge(e4);
        edgeBTL.count = 4;

        faceTree.setRoot(f);
        faceTree.count = 1;
    }

    //---------------------------------FOLDS------------------------------------------------//

    // Given 2 elements performs a mountain fold between the two, first is the one being moved
    public void mountainFold(VertexNode v1, VertexNode v2)
    {
        Plane p = createSymmetryPlane(v1, v2);
        symmetryFold(p, v1, camToModel);
    }
    public void mountainFold(VertexNode v1, Vector3 p1)
    {
        Plane p = createSymmetryPlane(v1, p1);
        symmetryFold(p, v1, camToModel);
    }
    public void mountainFold(Edge e1, Edge e2)
    {
        Plane p = createSymmetryPlane(e1, e2);
        symmetryFold(p, e1.origin, camToModel);
    }

    // Given 2 elements performs a valley fold between the two, first is the one being moved
    public void valleyFold(VertexNode v1, VertexNode v2)
    {
        Plane p = createSymmetryPlane(v1, v2);
        symmetryFold(p, v1, -camToModel);
    }
    public void valleyFold(VertexNode v1, Vector3 p1)
    {
        Plane p = createSymmetryPlane(v1, p1);
        symmetryFold(p, v1, -camToModel);
    }
    public void valleyFold(Edge e1, Edge e2)
    {
        Plane p = createSymmetryPlane(e1, e2);
        symmetryFold(p, e1.origin, -camToModel);
    }

    // Given 2 elements performs an open sink fold between the two, first is the one being moved
    public void openSinkFold(VertexNode v1, VertexNode v2)
    {
        Plane p = createSymmetryPlane(v1, v2);
        symmetryFold(p, v1, Vector3.zero);
    }
    public void openSinkFold(VertexNode v1, Vector3 p1)
    {
        Plane p = createSymmetryPlane(v1, p1);
        symmetryFold(p, v1, Vector3.zero);
    }
    public void openSinkFold(Edge e1, Edge e2)
    {
        Plane p = createSymmetryPlane(e1, e2);
        symmetryFold(p, e1.origin, Vector3.zero);
    }

    // Mountain, valley, inside and reverse folds and open sinks 
    public void symmetryFold(Plane p, VertexNode v1, Vector3 n)
    {
        step++;
        Vector3 delta = n * epsilon;
        p.addToNormal(delta);
        fold(p, v1);
    }

    // Unfolds the last step
    public void unfold()
    {
        step++;
        VertexNode v;
        Vertex vNew;
        int pos;
        List<VertexNode> vList = getVertices();
        for (int i = 0; i < vList.Count; i++)
        {
            pos = 1;
            v = vList[i];
            if ((v.peek(pos).step >= step - 1 - 2 * unfoldCount) && (v.vertices[0].step < step - 1 - 2 * unfoldCount))
            {
                while (v.peek(pos).step >= step - 1 - 2 * unfoldCount)
                {
                    pos++;
                }
                vNew = new Vertex(v.peek(pos).position, step,vertexLL.count);
                vertexLL.count++;
                v.Add(vNew);
            }
        }

        unfoldCount++;

    }

    // Given 2 elements i creates a crease between the two
    public void crease(VertexNode v1, VertexNode v2)
    {
        step++;
        Vector3 midpoint = (v1.getPosition() + v2.getPosition()) / 2;
        Vector3 v = v1.getPosition() - v2.getPosition();
        Vector3 n;
        FaceNode f = shareFace(v1, v2);
        if (f != null)
        {
            n = f.face.normal;
        }
        else
        {
            n = camToModel;
        }
        Plane p = new Plane(Vector3.Cross(v, n), midpoint);
        generateNewCreases(p);
    }
    public void crease(VertexNode v1, Vector3 p1)
    {
        step++;
        Vector3 midpoint = (v1.getPosition() + p1) / 2;
        Vector3 v = v1.getPosition() - p1;
        Vector3 n = camToModel;
        Plane p = new Plane(Vector3.Cross(v, n), midpoint);
        generateNewCreases(p);
    }

    // Given a vertex if folds it toa given point
    public void pull(VertexNode v1, Vector3 p1)
    {
        step++;
        Plane p = createSymmetryPlane(v1, p1);
        generateNewFlapCreases(p, v1);
        //generateNewCreases(p);
        List<VertexNode> aV = new List<VertexNode>();
        aV.Add(v1);
        applySymetry(p, aV);
        unfoldCount = 0;
    }

    // Given a vertex it moves it to the closest point to the selected not performing any creases
    public void displace(VertexNode vn, Vector3 p)
    {
        step++;
        Vector3 pos = new Vector3(0, 0, 0);

        List<Edge> eL = vn.edges;
        List<VertexNode> contiguous = new List<VertexNode>();

        for (int i = 0; i< eL.Count; i++)
        {
            if(eL[i].origin != vn)
            {
                contiguous.Add(eL[i].origin);
            }
            if (eL[i].end != vn)
            {
                contiguous.Add(eL[i].end);
            }
        }

        for (int i = 0; i < contiguous.Count; i++)
        {
            pos += contiguous[i].getPosition();
        }

        pos = pos / contiguous.Count;

        pos = vn.getPosition() + (pos - vn.getPosition()) * 2;



        /// 
        /// p1 = (x0,->c1-c2)
        /// q = p projectat p1
        /// p2 = (q, cross(->q-c1,->q-c2))
        /// 
        /// c = c1 +r1 -(r1+r2-d(c1,c2))/2
        /// rc = d(c, c2)
        /// r = sqrt(r2^2+rc^2)
        /// S = (c,r)

        //sage: x, y,z,a,b,c,d,e,f,g,h,i,j,k,l = var('x, y,z,a,b,c,d,e,f,g,h,i,j,k,l')
        //sage: solve([(x - a) ^ 2 + (y - b) ^ 2 + (z - c) ^ 2 == d, e * x + f * y + z * g == h, i * x + j * y + z * k == l], x, y,z)

        ////x == -((c * f * g - b * g ^ 2 - f * h) * i * j - (c * e * g - a * g ^ 2 - e * h) * j ^ 2 - (b * e * f - a * f ^ 2 - e * h) * k ^ 2 - ((c * f ^ 2 - b * f * g + g * h) * i - (c * e * f + (b * e - 2 * a * f) * g) * j) * k - (e * f * j + e * g * k - (f ^ 2 + g ^ 2) * i) * l - sqrt(-(2 * b * c * f * g + (a ^ 2 + b ^ 2 - d) * f ^ 2 + (a ^ 2 + c ^ 2 - d) * g ^ 2 - 2 * (b * f + c * g) * h + h ^ 2) * i ^ 2 - 2 * (a * b * g ^ 2 - (a ^ 2 + b ^ 2 - d) * e * f - (b * c * e + a * c * f) * g + (b * e + a * f) * h) * i * j - (2 * a * c * e * g + (a ^ 2 + b ^ 2 - d) * e ^ 2 + (b ^ 2 + c ^ 2 - d) * g ^ 2 - 2 * (a * e + c * g) * h + h ^ 2) * j ^ 2 - (2 * a * b * e * f + (a ^ 2 + c ^ 2 - d) * e ^ 2 + (b ^ 2 + c ^ 2 - d) * f ^ 2 - 2 * (a * e + b * f) * h + h ^ 2) * k ^ 2 - (e ^ 2 + f ^ 2 + g ^ 2) * l ^ 2 + 2 * ((b * c * e * f - a * c * f ^ 2 + (a * b * f + (a ^ 2 + c ^ 2 - d) * e) * g - (c * e + a * g) * h) * i - (b * c * e ^ 2 - a * c * e * f - (a * b * e + (b ^ 2 + c ^ 2 - d) * f) * g + (c * f + b * g) * h) * j) * k - 2 * ((b * e * f - a * f ^ 2 + c * e * g - a * g ^ 2 - e * h) * i - (b * e ^ 2 - a * e * f - c * f * g + b * g ^ 2 + f * h) * j - (c * e ^ 2 + c * f ^ 2 - (a * e + b * f) * g + g * h) * k) * l) * (g * j - f * k)) / (2 * e * f * i * j - (f ^ 2 + g ^ 2) * i ^ 2 - (e ^ 2 + g ^ 2) * j ^ 2 - (e ^ 2 + f ^ 2) * k ^ 2 + 2 * (e * g * i + f * g * j) * k), 
        ///y == ((c * f * g - b * g ^ 2 - f * h) * i ^ 2 - (c * e * g - a * g ^ 2 - e * h) * i * j - (b * e ^ 2 - a * e * f + f * h) * k ^ 2 - ((c * e * f - (2 * b * e - a * f) * g) * i - (c * e ^ 2 - a * e * g + g * h) * j) * k + (e * f * i + f * g * k - (e ^ 2 + g ^ 2) * j) * l - sqrt(-(2 * b * c * f * g + (a ^ 2 + b ^ 2 - d) * f ^ 2 + (a ^ 2 + c ^ 2 - d) * g ^ 2 - 2 * (b * f + c * g) * h + h ^ 2) * i ^ 2 - 2 * (a * b * g ^ 2 - (a ^ 2 + b ^ 2 - d) * e * f - (b * c * e + a * c * f) * g + (b * e + a * f) * h) * i * j - (2 * a * c * e * g + (a ^ 2 + b ^ 2 - d) * e ^ 2 + (b ^ 2 + c ^ 2 - d) * g ^ 2 - 2 * (a * e + c * g) * h + h ^ 2) * j ^ 2 - (2 * a * b * e * f + (a ^ 2 + c ^ 2 - d) * e ^ 2 + (b ^ 2 + c ^ 2 - d) * f ^ 2 - 2 * (a * e + b * f) * h + h ^ 2) * k ^ 2 - (e ^ 2 + f ^ 2 + g ^ 2) * l ^ 2 + 2 * ((b * c * e * f - a * c * f ^ 2 + (a * b * f + (a ^ 2 + c ^ 2 - d) * e) * g - (c * e + a * g) * h) * i - (b * c * e ^ 2 - a * c * e * f - (a * b * e + (b ^ 2 + c ^ 2 - d) * f) * g + (c * f + b * g) * h) * j) * k - 2 * ((b * e * f - a * f ^ 2 + c * e * g - a * g ^ 2 - e * h) * i - (b * e ^ 2 - a * e * f - c * f * g + b * g ^ 2 + f * h) * j - (c * e ^ 2 + c * f ^ 2 - (a * e + b * f) * g + g * h) * k) * l) * (g * i - e * k)) / (2 * e * f * i * j - (f ^ 2 + g ^ 2) * i ^ 2 - (e ^ 2 + g ^ 2) * j ^ 2 - (e ^ 2 + f ^ 2) * k ^ 2 + 2 * (e * g * i + f * g * j) * k), 
        ///z == -((c * f ^ 2 - b * f * g + g * h) * i ^ 2 - (2 * c * e * f - (b * e + a * f) * g) * i * j + (c * e ^ 2 - a * e * g + g * h) * j ^ 2 + ((b * e * f - a * f ^ 2 - e * h) * i - (b * e ^ 2 - a * e * f + f * h) * j) * k - (e * g * i + f * g * j - (e ^ 2 + f ^ 2) * k) * l - sqrt(-(2 * b * c * f * g + (a ^ 2 + b ^ 2 - d) * f ^ 2 + (a ^ 2 + c ^ 2 - d) * g ^ 2 - 2 * (b * f + c * g) * h + h ^ 2) * i ^ 2 - 2 * (a * b * g ^ 2 - (a ^ 2 + b ^ 2 - d) * e * f - (b * c * e + a * c * f) * g + (b * e + a * f) * h) * i * j - (2 * a * c * e * g + (a ^ 2 + b ^ 2 - d) * e ^ 2 + (b ^ 2 + c ^ 2 - d) * g ^ 2 - 2 * (a * e + c * g) * h + h ^ 2) * j ^ 2 - (2 * a * b * e * f + (a ^ 2 + c ^ 2 - d) * e ^ 2 + (b ^ 2 + c ^ 2 - d) * f ^ 2 - 2 * (a * e + b * f) * h + h ^ 2) * k ^ 2 - (e ^ 2 + f ^ 2 + g ^ 2) * l ^ 2 + 2 * ((b * c * e * f - a * c * f ^ 2 + (a * b * f + (a ^ 2 + c ^ 2 - d) * e) * g - (c * e + a * g) * h) * i - (b * c * e ^ 2 - a * c * e * f - (a * b * e + (b ^ 2 + c ^ 2 - d) * f) * g + (c * f + b * g) * h) * j) * k - 2 * ((b * e * f - a * f ^ 2 + c * e * g - a * g ^ 2 - e * h) * i - (b * e ^ 2 - a * e * f - c * f * g + b * g ^ 2 + f * h) * j - (c * e ^ 2 + c * f ^ 2 - (a * e + b * f) * g + g * h) * k) * l) * (f * i - e * j)) / (2 * e * f * i * j - (f ^ 2 + g ^ 2) * i ^ 2 - (e ^ 2 + g ^ 2) * j ^ 2 - (e ^ 2 + f ^ 2) * k ^ 2 + 2 * (e * g * i + f * g * j) * k)], 
        ///[x == -((c* f* g - b* g^2 - f* h)*i* j - (c* e* g - a* g^2 - e* h)*j^2 - (b* e* f - a* f^2 - e* h)*k^2 - ((c* f^2 - b* f*g + g* h)*i - (c* e* f + (b* e - 2*a* f)*g)*j)*k - (e* f* j + e* g*k - (f^2 + g^2)*i)*l + sqrt(-(2*b* c*f* g + (a^2 + b^2 - d)*f^2 + (a^2 + c^2 - d)*g^2 - 2*(b* f + c* g)*h + h^2)*i^2 - 2*(a* b* g^2 - (a^2 + b^2 - d)*e* f - (b* c* e + a* c*f)*g + (b* e + a* f)*h)*i* j - (2*a* c*e* g + (a^2 + b^2 - d)*e^2 + (b^2 + c^2 - d)*g^2 - 2*(a* e + c* g)*h + h^2)*j^2 - (2*a* b*e* f + (a^2 + c^2 - d)*e^2 + (b^2 + c^2 - d)*f^2 - 2*(a* e + b* f)*h + h^2)*k^2 - (e^2 + f^2 + g^2)*l^2 + 2*((b* c* e* f - a* c*f^2 + (a* b* f + (a^2 + c^2 - d)*e)*g - (c* e + a* g)*h)*i - (b* c* e^2 - a* c*e* f - (a* b* e + (b^2 + c^2 - d)*f)*g + (c* f + b* g)*h)*j)*k - 2*((b* e* f - a* f^2 + c* e*g - a* g^2 - e* h)*i - (b* e^2 - a* e*f - c* f*g + b* g^2 + f* h)*j - (c* e^2 + c* f^2 - (a* e + b* f)*g + g* h)*k)*l)*(g* j - f* k))/(2*e* f*i* j - (f^2 + g^2)*i^2 - (e^2 + g^2)*j^2 - (e^2 + f^2)*k^2 + 2*(e* g* i + f* g*j)*k), 
        ///y == ((c* f* g - b* g^2 - f* h)*i^2 - (c* e* g - a* g^2 - e* h)*i* j - (b* e^2 - a* e*f + f* h)*k^2 - ((c* e* f - (2*b* e - a* f)*g)*i - (c* e^2 - a* e*g + g* h)*j)*k + (e* f* i + f* g*k - (e^2 + g^2)*j)*l + sqrt(-(2*b* c*f* g + (a^2 + b^2 - d)*f^2 + (a^2 + c^2 - d)*g^2 - 2*(b* f + c* g)*h + h^2)*i^2 - 2*(a* b* g^2 - (a^2 + b^2 - d)*e* f - (b* c* e + a* c*f)*g + (b* e + a* f)*h)*i* j - (2*a* c*e* g + (a^2 + b^2 - d)*e^2 + (b^2 + c^2 - d)*g^2 - 2*(a* e + c* g)*h + h^2)*j^2 - (2*a* b*e* f + (a^2 + c^2 - d)*e^2 + (b^2 + c^2 - d)*f^2 - 2*(a* e + b* f)*h + h^2)*k^2 - (e^2 + f^2 + g^2)*l^2 + 2*((b* c* e* f - a* c*f^2 + (a* b* f + (a^2 + c^2 - d)*e)*g - (c* e + a* g)*h)*i - (b* c* e^2 - a* c*e* f - (a* b* e + (b^2 + c^2 - d)*f)*g + (c* f + b* g)*h)*j)*k - 2*((b* e* f - a* f^2 + c* e*g - a* g^2 - e* h)*i - (b* e^2 - a* e*f - c* f*g + b* g^2 + f* h)*j - (c* e^2 + c* f^2 - (a* e + b* f)*g + g* h)*k)*l)*(g* i - e* k))/(2*e* f*i* j - (f^2 + g^2)*i^2 - (e^2 + g^2)*j^2 - (e^2 + f^2)*k^2 + 2*(e* g* i + f* g*j)*k), 
        ///z == -((c* f^2 - b* f*g + g* h)*i^2 - (2*c* e*f - (b* e + a* f)*g)*i* j + (c* e^2 - a* e*g + g* h)*j^2 + ((b* e* f - a* f^2 - e* h)*i - (b* e^2 - a* e*f + f* h)*j)*k - (e* g* i + f* g*j - (e^2 + f^2)*k)*l + sqrt(-(2*b* c*f* g + (a^2 + b^2 - d)*f^2 + (a^2 + c^2 - d)*g^2 - 2*(b* f + c* g)*h + h^2)*i^2 - 2*(a* b* g^2 - (a^2 + b^2 - d)*e* f - (b* c* e + a* c*f)*g + (b* e + a* f)*h)*i* j - (2*a* c*e* g + (a^2 + b^2 - d)*e^2 + (b^2 + c^2 - d)*g^2 - 2*(a* e + c* g)*h + h^2)*j^2 - (2*a* b*e* f + (a^2 + c^2 - d)*e^2 + (b^2 + c^2 - d)*f^2 - 2*(a* e + b* f)*h + h^2)*k^2 - (e^2 + f^2 + g^2)*l^2 + 2*((b* c* e* f - a* c*f^2 + (a* b* f + (a^2 + c^2 - d)*e)*g - (c* e + a* g)*h)*i - (b* c* e^2 - a* c*e* f - (a* b* e + (b^2 + c^2 - d)*f)*g + (c* f + b* g)*h)*j)*k - 2*((b* e* f - a* f^2 + c* e*g - a* g^2 - e* h)*i - (b* e^2 - a* e*f - c* f*g + b* g^2 + f* h)*j - (c* e^2 + c* f^2 - (a* e + b* f)*g + g* h)*k)*l)*(f* i - e* j))/(2*e* f*i* j - (f^2 + g^2)*i^2 - (e^2 + g^2)*j^2 - (e^2 + f^2)*k^2 + 2*(e* g* i + f* g*j)*k)



        Vertex newV = new Vertex(pos, step, vertexLL.count);
        vertexLL.count++;
        vn.Add(newV);
        unfoldCount = 0;
    }

    //---------------------------------AUXILIAR FUNCTIONS-------------------------------------------// 

    // Given 2 elements it creates the symmetry plane for the fold
    public Plane createSymmetryPlane(VertexNode v1, VertexNode v2)
    {
        Vector3 normal = v1.getPosition() - v2.getPosition();
        Vector3 midpoint = (v1.getPosition() + v2.getPosition()) / 2;
        Plane p = new Plane(normal, midpoint);
        return p;
    }
    public Plane createSymmetryPlane(Edge e1, Edge e2)
    {
        Vector3 v1 = coplanar(e1, e2);
        if (v1 != Vector3.zero)
        {
            Vector3 v2 = bisector(e1, e2);
            Vector3 n = Vector3.Cross(v1, v2);

            Vector3 point = cut(e1, e2);

            Plane p = new Plane(n, point);
            return p;
        }
        else
        {
            return null;
        }
    }
    public Plane createSymmetryPlane(VertexNode v1, Vector3 p1)
    {
        Vector3 midpoint = (v1.getPosition() + p1) / 2;
        Vector3 v = v1.getPosition() - p1;

        return new Plane(v, midpoint);
    }

    // Folds along a symmetry plane v being the moved vertex
    public void fold(Plane p, VertexNode v)
    {
        List<VertexNode> aV = getAfecctedVertices(p, v);

        generateNewCreases(p);

        applySymetry(p, aV);

        unfoldCount = 0;
    }

    // Creates vertices, edges and faces and inserts them in the model's strucutre
    public void generateNewCreases(Plane p)
    {
        List<EdgeNode> activeEdgeNodes = edgeBTL.getLeaveNodes();
        List<VertexNode> activeVertices = getVertices();
        List<VertexNode> prevCutVertices = new List<VertexNode>();
        List<VertexNode> cutVertices = new List<VertexNode>();
        /// si tallo un vertex el tinc en compte d(etermineSide =0)
        for (int i = 0; i < activeVertices.Count; i++)
        {
            if (p.determineSide(activeVertices[i].peek()) == 0)
            {
                cutVertices.Add(activeVertices[i]);
                prevCutVertices.Add(activeVertices[i]);
            }
        }
        //prevCutVertices = cutVertices;
        List<VertexNode> vNode = getVertices();
 
        /// per cada aresta que es talla faig split i genero un vertex
        for (int i = 0; i < activeEdgeNodes.Count; i++)
        {
            EdgeNode parent = activeEdgeNodes[i];
            VertexNode o = parent.edge.origin;
            VertexNode e = parent.edge.end;


            Vector3 point = parent.edge.isCut(p);

            Vector3 inf = new Vector3(9999, 9999, 9999);

            bool close = isClose(point, prevCutVertices);

            if (point != inf && !close)
            {
                Vertex v = new Vertex(point, step, vertexLL.vll.Count);
                vertexLL.count++;
                VertexNode vn = new VertexNode(v);
                cutVertices.Add(vn);
                vertexLL.addVertex(vn);

                Edge edgel = new Edge(o, vn, step, edgeBTL.count);
                edgeBTL.count ++;
                EdgeNode ls = new EdgeNode(parent, edgel);
                vn.addEdge(edgel);
                edgel.origin.edges.Remove(parent.edge);
                edgel.origin.addEdge(edgel);

                Edge edger = new Edge(vn, e,  step, edgeBTL.count);
                edgeBTL.count++;
                EdgeNode rs = new EdgeNode(parent, edger);
                vn.addEdge(edger);
                edger.end.edges.Remove(parent.edge);
                edger.end.addEdge(edger);

                for (int j = 0; j< parent.edge.faces.Count; j++)
                {
                    parent.edge.faces[j].extraV.Add(vn);
                }

                parent.setSons(ls, rs);
            }
        }

        /// per cada vertex que comparteix cara 2 a 2 genero una aresta
        for (int i = 0; i < cutVertices.Count; i++)
        {
            for (int j = i + 1; j < cutVertices.Count; j++)
            {
                FaceNode f = shareFace(cutVertices[i], cutVertices[j]);
                if (f != null && !doesEdgeExist(cutVertices[i], cutVertices[j]))
                {
                    Edge e = new Edge(cutVertices[i], cutVertices[j], step , edgeBTL.count);
                    edgeBTL.count++;
                    cutVertices[i].addEdge(e);
                    cutVertices[j].addEdge(e);

                    edgeBTL.addEdge(e);

                    List<VertexNode> l = f.face.vList;
                    List<VertexNode> l1 = new List<VertexNode>();
                    List<VertexNode> l2 = new List<VertexNode>();
                    int side;

                    l1.Add(cutVertices[i]);
                    l2.Add(cutVertices[i]);
                    l1.Add(cutVertices[j]);
                    l2.Add(cutVertices[j]);

                    for (int k = 0; k < l.Count; k++)
                    {
                        side = p.determineSide(l[k].peek());
                        if (side == 1)
                        {
                            l1.Add(l[k]);
                        }
                        else if (side == -1)
                        {
                            l2.Add(l[k]);
                        }
                    }
                    Face f1 = new Face(l1, step,faceTree.count);
                    faceTree.count++;
                    Face f2 = new Face(l2, step, faceTree.count);
                    faceTree.count++;


                    FaceNode fn1 = new FaceNode(f, f1);
                    FaceNode fn2 = new FaceNode(f, f2);

                    for (int k = 0; k< l1.Count; k++)
                    {
                        for (int m = k+1; m < l1.Count; m++)
                        {
                            Edge foundEdge = edgeExists(l1[k], l1[m]);
                            if (foundEdge != null)
                            {
                                foundEdge.faces.Add(f1);
                            }
                        }
                    }
                    for (int k = 0; k < l2.Count; k++)
                    {
                        for (int m = k + 1; m < l2.Count; m++)
                        {
                            Edge foundEdge = edgeExists(l2[k], l2[m]);
                            if (foundEdge != null)
                            {
                                foundEdge.faces.Add(f2);
                            }
                        }
                    }
                }
            }
        }
        List<Face> faces = getFaces();
        
        for (int i = 0; i< faces.Count; i++)
        {

            //faces[i].sortVertices();
        }

        List<Edge> activeE = getEdges();
       
        for (int i = 0; i< activeE.Count;i++)
        {
            activeE[i].cleanFaces(faces);
        }
    }

    public void generateNewFlapCreases(Plane p, VertexNode v1)
    {
        List<EdgeNode> activeEdgeNodes = edgeBTL.getLeaveNodes();
        List<VertexNode> activeVertices = getVertices();
        List<Edge> flapEdges = v1.edges;
        List<VertexNode> prevCutVertices = new List<VertexNode>();


        List<VertexNode> cutVertices = new List<VertexNode>();
        /// si tallo un vertex el tinc en compte d(etermineSide =0)
        for (int i = 0; i < activeVertices.Count; i++)
        {
            if (p.determineSide(activeVertices[i].peek()) == 0)
            {
                cutVertices.Add(activeVertices[i]);
                prevCutVertices.Add(activeVertices[i]);
            }
        }
        
        /// per cada aresta que es talla faig split i genero un vertex
        for (int i = 0; i < activeEdgeNodes.Count; i++)
        {
            if (flapEdges.Contains(activeEdgeNodes[i].edge))
            {
                EdgeNode parent = activeEdgeNodes[i];
                VertexNode o = parent.edge.origin;
                VertexNode e = parent.edge.end;


                Vector3 point = parent.edge.isCut(p);

                Vector3 inf = new Vector3(9999, 9999, 9999);

                bool close = isClose(point, prevCutVertices);

                if (point != inf && !close) //
                {
                    Vertex v = new Vertex(point, step, vertexLL.vll.Count);
                    vertexLL.count++;
                    VertexNode vn = new VertexNode(v);
                    cutVertices.Add(vn);
                    vertexLL.addVertex(vn);

                    Edge edgel = new Edge(o, vn, step, edgeBTL.count);
                    edgeBTL.count++;
                    EdgeNode ls = new EdgeNode(parent, edgel);
                    vn.addEdge(edgel);
                    edgel.origin.edges.Remove(parent.edge);
                    edgel.origin.addEdge(edgel);

                    Edge edger = new Edge(vn, e, step, edgeBTL.count);
                    edgeBTL.count++;
                    EdgeNode rs = new EdgeNode(parent, edger);
                    vn.addEdge(edger);
                    edger.end.edges.Remove(parent.edge);
                    edger.end.addEdge(edger);

                    for (int j = 0; j < parent.edge.faces.Count; j++)
                    {
                        parent.edge.faces[j].extraV.Add(vn);
                    }

                    parent.setSons(ls, rs);
                }
            }
            
        }

        /// per cada vertex que comparteix cara 2 a 2 genero una aresta
        for (int i = 0; i < cutVertices.Count; i++)
        {
            for (int j = i + 1; j < cutVertices.Count; j++)
            {
                FaceNode f = shareFace(cutVertices[i], cutVertices[j]);
                if (f != null && !doesEdgeExist(cutVertices[i], cutVertices[j]))
                {
                    Edge e = new Edge(cutVertices[i], cutVertices[j], step, edgeBTL.count);
                    edgeBTL.count++;
                    cutVertices[i].addEdge(e);
                    cutVertices[j].addEdge(e);

                    edgeBTL.addEdge(e);

                    List<VertexNode> l = f.face.vList;
                    List<VertexNode> l1 = new List<VertexNode>();
                    List<VertexNode> l2 = new List<VertexNode>();
                    int side;

                    l1.Add(cutVertices[i]);
                    l2.Add(cutVertices[i]);
                    l1.Add(cutVertices[j]);
                    l2.Add(cutVertices[j]);

                    for (int k = 0; k < l.Count; k++)
                    {
                        side = p.determineSide(l[k].peek());
                        if (side == 1)
                        {
                            l1.Add(l[k]);
                        }
                        else if (side == -1)
                        {
                            l2.Add(l[k]);
                        }
                    }
                    Face f1 = new Face(l1, step, faceTree.count);
                    faceTree.count++;
                    Face f2 = new Face(l2, step, faceTree.count);
                    faceTree.count++;


                    FaceNode fn1 = new FaceNode(f, f1);
                    FaceNode fn2 = new FaceNode(f, f2);

                    for (int k = 0; k < l1.Count; k++)
                    {
                        for (int m = k + 1; m < l1.Count; m++)
                        {
                            Edge foundEdge = edgeExists(l1[k], l1[m]);
                            if (foundEdge != null)
                            {
                                foundEdge.faces.Add(f1);
                            }
                        }
                    }
                    for (int k = 0; k < l2.Count; k++)
                    {
                        for (int m = k + 1; m < l2.Count; m++)
                        {
                            Edge foundEdge = edgeExists(l2[k], l2[m]);
                            if (foundEdge != null)
                            {
                                foundEdge.faces.Add(f2);
                            }
                        }
                    }
                }
            }
        }
        List<Face> faces = getFaces();

        for (int i = 0; i < faces.Count; i++)
        {

            //faces[i].sortVertices();
        }

        List<Edge> activeE = getEdges();

        for (int i = 0; i < activeE.Count; i++)
        {
            activeE[i].cleanFaces(faces);
        }
    }

    public bool isClose(Vector3 p, List<VertexNode> vl)
    {
        bool a = false;
        for(int i = 0; i<vl.Count; i++)
        {
            if(0.01>Vector3.Distance(p, vl[i].getPosition()))
            {
                a = true;
            }
        }
        return a;
    }


    // Applies a specular symmetry across the plain to each vertex and pushes it into the structure
    public void applySymetry(Plane p, List<VertexNode> vL)
    {
        Matrix4x4 C, S, Cinv, M;
        Vector4 v1 = new Vector4(p.n.x, p.n.y, p.n.z, 0);
        Vector4 v2 = new Vector4(p.vd1.x, p.vd1.y, p.vd1.z, 0);
        Vector4 v3 = new Vector4(p.vd2.x, p.vd2.y, p.vd2.z, 0);
        Vector3 planep = p.point;
        Vector4 v4 = new Vector4(planep.x, planep.y, planep.z, 1);
        C = new Matrix4x4(v1, v2, v3, v4);
        Cinv = C.inverse;
        S = Matrix4x4.identity;
        S.m00 = -1;

        Vector4 q;

        M = C * S * Cinv;

        for (int i = 0; i < vL.Count; i++)
        {
            q = new Vector4(vL[i].peek().position.x, vL[i].peek().position.y, vL[i].peek().position.z, 1);
            Vector4 point = M.MultiplyPoint(q);
            Vertex newV = new Vertex(new Vector3(point.x, point.y, point.z), step, vertexLL.count);
            vertexLL.count++;
            vL[i].Add(newV);
        }
    }

    // Discerns whether two edges are coplanar. If they are it retuns the normal vector  
    public Vector3 coplanar(Edge e1, Edge e2)
    {
        Vector3 i = intersect(e1, e2);
        Vector3 v1 = e1.getNormalizedVector();
        Vector3 v2 = e2.getNormalizedVector();
        //they intersect
        if (i != Vector3.zero)
        {
            return Vector3.Cross(v1, v2);
        }
        // they're parallel
        else if (v1 == v2 || v1 == -v2)
        {
            Vector3 p1 = e1.origin.getPosition();
            Vector3 p2 = e2.origin.getPosition();
            if (p1 != p2)
            {
                v2 = p1 - p2;
            }
            else
            {
                p2 = e2.end.getPosition();
                v2 = p1 - p2;
            }
            return Vector3.Cross(v1, v2);
        }
        return Vector3.zero;
    }

    // Returns the bisector angle of the two vertices
    public Vector3 bisector(Edge e1, Edge e2)
    {
        Vector3 v1 = e1.getNormalizedVector();
        Vector3 v2 = e2.getNormalizedVector();
        if (v1 == v2)
        {
            return v1;
        }
        else
        {
            return (v1 - v2).normalized;
        }
    }

    // if the edges cut returns the point, else a point in their bisector
    public Vector3 cut(Edge e1, Edge e2)
    {
        Vector3 i = intersect(e1, e2);
        ////////////////noooooooooooooooooooooooo ha de ser diferent perque es un punt
        if (i != Vector3.zero)
        {
            return i;
        }
        else
        {
            Vector3 p1 = e1.origin.getPosition();
            Vector3 p2 = e2.origin.getPosition();
            if (p1 != p2)
            {
                i = (p1 + p2) / 2;
                return i;
            }
            else
            {
                p2 = e2.end.getPosition();
                i = (p1 + p2) / 2;
                return i;
            }
        }
    }

    // Discerns if the two edges prolongued cross at some point and returns the point if it exists
    public Vector3 intersect(Edge e1, Edge e2)
    {

        Vector3 v1 = e1.getNormalizedVector();
        Vector3 v2 = e2.getNormalizedVector();
        //parallel
        if (v1 == v2)
        {
            //noooooooooooooooooooooo
            return Vector3.zero;
        }
        else
        {
            Vector3 p1 = e1.origin.getPosition();
            Vector3 p2 = e2.origin.getPosition();
            if (p1 != p2)
            {
                float t, num, denom;
                denom = (-v1.x * v2.y + v1.y * v2.x);
                num = -(p2.x - p1.x) * v2.y + (p2.y - p1.y) * v2.x;
                if (denom == 0)
                {
                    return Vector3.zero;
                }
                else
                {
                    t = num / denom;
                }
                return p1 + t * v1;
            }
            else
            {
                return p1;
            }
        }
    }

    // Returns wether the 2 vertices share a face and returns it if it exists
    public FaceNode shareFace(VertexNode v1, VertexNode v2)
    {
        FaceNode f = null;
        List<FaceNode> faceNodes = faceTree.getFaceNodes();
        for (int i = 0; i < faceNodes.Count; i++)
        {
            
            if (faceNodes[i].face.has(v1) && faceNodes[i].face.has(v2))
            {
                f = faceNodes[i];
                return f;
            }
        }

        //List<Face> e1 = new List<Face>();
        //List<Face> e2 = new List<Face>();

        //for (int i = 0; i < v1.edges.Count; i++)
        //{
        //    Edge e = v1.edges[i];
        //    for (int j = 0; j < e.faces.Count; j++)
        //    {
        //        e1.Add(e.faces[j]);
        //    }
        //}

        //for (int i = 0; i < v2.edges.Count; i++)
        //{
        //    Edge e = v2.edges[i];
        //    for (int j = 0; j < e.faces.Count; j++)
        //    {
        //        e2.Add(e.faces[j]);
        //    }
        //}

        //for (int i = 0; i < e1.Count; i++)
        //{
        //    for (int j = 0; j < e2.Count; j++)
        //    {
        //        if (e1[i] == e2[j])
        //        {
        //            return faceTree.findNode(e1[i]);
        //        }
        //    }
        //}

        return f;
    }

    public Edge edgeExists (VertexNode v1, VertexNode v2)
    {
        Edge e = null;
        List<Edge> el = getEdges();
        for (int i = 0; i<el.Count; i++)
        {
            
            if(v1 == el[i].origin && v2 == el[i].end)
            {
                e = el[i];
            }
            if (v2 == el[i].origin && v1 == el[i].end)
            {
                e = el[i];
            }
        }
        return e;
    }

    public bool doesEdgeExist(VertexNode v1, VertexNode v2)
    {
        bool b= false;
        List<Edge> el = getEdges();
        for (int i = 0; i < el.Count; i++)
        {

            if (v1 == el[i].origin && v2 == el[i].end)
            {
                b = true;
            }
            if (v2 == el[i].origin && v1 == el[i].end)
            {
                b = true;
            }
        }
        return b;
    }

    // Returns all the vertices on one side of the plane (v's side)
    public List<VertexNode> getAfecctedVertices(Plane p, VertexNode v)
    {
        List<VertexNode> vertices = getVertices();
        int side = p.determineSide(v.peek());
        List<VertexNode> affectedV = new List<VertexNode>();

        for (int i = 0; i < vertices.Count; i++)
        {
            if (p.determineSide(vertices[i].peek()) == side)
            {
                affectedV.Add(vertices[i]);
            }
        }
        return affectedV;
    }

    // Gets all active faces
    public List<Face> getFaces()
    {
        return faceTree.getFaces();
    }
    // Gets all active edges
    public List<Edge> getEdges()
    {
        return edgeBTL.getEdges();
    }
    // Gets all active vertices
    public List<VertexNode> getVertices()
    {
        return vertexLL.getVertices();
    }

    // Gets all active faces in step n
    public List<Face> getFaces(int n)
    {
        return faceTree.getFaces(n);
    }
    // Gets all active edges in step n
    public List<Edge> getEdges(int n)
    {
        return edgeBTL.getEdges(n);
    }
    // Gets all active vertices in step n
    public List<VertexNode> getVertices(int n)
    {
        return vertexLL.getVertices(n);
    }

    override
    public string ToString ()
    {
        List<VertexNode> v = getVertices();
        List<Face> f = getFaces();
        List<Edge> e = getEdges();


        string s = "V: "+v.Count+ " E: "+e.Count+" F: "+f.Count+"\n";
        s += "Vertices: " + v.Count + " \n";
        for (int i = 0; i < v.Count; i++)
        {
            s += v[i].ToString();
            s += '\n';
        }
        
        s += "Edges: "+e.Count+" \n";      
        for (int i = 0; i < e.Count; i++)
        { 
            s += e[i].ToString();
            s += '\n';
        }
        
        s += "Faces: "+f.Count+" \n";
        for (int i=0;i<f.Count; i++)
        {
            s += f[i].ToString();
            s += '\n';
        }
        return s;
    }
}