using UnityEngine;

/// This class represents geometrycally a plane 
public class Plane
{
    public float A, B, C, D;
    public Vector3 n, vd1, vd2, point;

    // Given a normal vector and a point coefficients are calculated and two vectors contained in the plane
    public Plane(Vector3 normal, Vector3 p)
    {
        A = normal.x;
        B = normal.y;
        C = normal.z;
        D = A * p.x + B * p.y + C * p.z;
        n = normal;
        point = p;

        calculateDirectorVectors(p);
    }

    // Determines wether a point is in one side, the other or contained in it
    public int determineSide(Vertex v)
    {
        if (Mathf.Abs(A * v.position.x + B * v.position.y + C * v.position.z - D) <= 0.05 ) return 0;
        else if (A * v.position.x + B * v.position.y + C * v.position.z - D < 0) return -1;
        else return 1;

    }

    // Returns a random point in the plane
    public Vector3 getRandomPoint()
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
            point = new Vector3(n, (D - A * n - C * m) / B, m);
        }
        else
        {
            point = new Vector3((D - B * n - C * m) / A, n, m);
        }

        return point;

    }

    // It calculates the director vectors of the plane
    public void calculateDirectorVectors(Vector3 p)
    {
        vd1 = p - getRandomPoint();
        vd2 = p - getRandomPoint();
        vd1 = vd1.normalized;
        vd2 = vd2.normalized;
        while (vd2 == vd1)
        {
            vd2 = p - getRandomPoint();
            vd2 = vd2.normalized;
        }
    }

    // Changes the plane adding a vector to the normal and recalculating it
    public void addToNormal(Vector3 delta)
    {
        n = n + delta;
        calculateCoef();
        calculateDirectorVectors(point);
    }

    // Calculates the coeficient of the plane equation
    public void calculateCoef()
    {
        A = n.x;
        B = n.y;
        C = n.z;
        D = A * point.x + B * point.y + C * point.z;
    }
}