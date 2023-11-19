using System;
using ToonBlastClone.Structs;
using UnityEngine;

namespace ToonBlastClone.Logic
{
    public static class Calculator
    {
        // Möller-Trumbore algorithm
        public static bool TriangleIntersect(Vector3 p1, Vector3 p2, Vector3 p3,
            ScreenToWorldPointData screenToWorldPoint)
        {
            // Vectors from p1 to p2/p3 (edges)
            Vector3 e1, e2;

            Vector3 p, q, t;
            float det, invDet, u, v;


            //Find vectors for two edges sharing vertex/point p1
            e1 = p2 - p1;
            e2 = p3 - p1;

            // calculating determinant 
            p = Vector3.Cross(screenToWorldPoint.Direction, e2);

            //Calculate determinat
            det = Vector3.Dot(e1, p);

            //if determinant is near zero, ray lies in plane of triangle otherwise not
            if (det > -Double.Epsilon && det < Double.Epsilon)
            {
                return false;
            }

            invDet = 1.0f / det;

            //calculate distance from p1 to ray origin
            t = screenToWorldPoint.Origin - p1;

            //Calculate u parameter
            u = Vector3.Dot(t, p) * invDet;

            //Check for ray hit
            if (u < 0 || u > 1)
            {
                return false;
            }

            //Prepare to test v parameter
            q = Vector3.Cross(t, e1);

            //Calculate v parameter
            v = Vector3.Dot(screenToWorldPoint.Direction, q) * invDet;

            //Check for ray hit
            if (v < 0 || u + v > 1)
            {
                return false;
            }

            if ((Vector3.Dot(e2, q) * invDet) > Double.Epsilon)
            {
                //ray does intersect
                return true;
            }

            // No hit at all
            return false;
        }

        // public static bool RayPlaneIntersection(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,
        //     ScreenToWorldPointData screenToWorldPoint)
        // {
        //     
        // }
    }
}