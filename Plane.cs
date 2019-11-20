using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// This class represents geometrycally a plane 
public class Plane
{
    public float A, B, C, D;
    public Vector3 n, vd1, vd2;

    // Given a normal vector and a point coefficients are calculated and two vectors contained in the plane
    public Plane(Vector3 normal, Vector3 p)
    {
        A = normal.x;
        B = normal.y;
        C = normal.z;
        D = A * p.x + B * p.y + C * p.z;
        n = normal;

        vd1 = p - getPoint();
        vd2 = p - getPoint();
        vd1 = vd1.normalized;
        vd2 = vd2.normalized;
        while (vd2 == vd1)
        {
            vd2 = p - getPoint();
            vd2 = vd2.normalized;
        }
    }
    // Determines wether a point is in one side, the other or contained in it
    public int determineSide(Vertex v)
    {
        if (A * v.position.x + B * v.position.y + C * v.position.z == D) return 0;
        if (A * v.position.x + B * v.position.y + C * v.position.z < D) return -1;
        else return 1;

    }
    // Returns a random point in the plane
    public Vector3 getPoint()
    {
        Vector3 point;
        int n, m;
        n = Random.Range(1, 100);
        m = Random.Range(1, 100);

        if (C != 0)
        {
            point = new Vector3(n, m, (D - A * n - B * m) / C);
        }
        else if (B != 0)
        {
            point = new Vector3(n, (D - A * n - C * m )/ B, m);
        }
        else
        {
            point = new Vector3((D - B * n - C * m )/ A, n, m);
        }

        return point;
    }
}
