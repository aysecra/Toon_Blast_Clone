using ToonBlastClone.Structs;
using UnityEngine;

namespace ToonBlastClone.Data
{
    public class PlaneBorderData
    {
        public Vector3 CenterPosition { get; }
        public Vector3 MinPosition { get; }
        public Vector3 MaxPosition { get; }

        public PlaneBorderData(Vector3 center, Vector3 minPosition, Vector3 maxPosition)
        {
            CenterPosition = center;
            MinPosition = minPosition;
            MaxPosition = maxPosition;
        }

        public Vector2Int PlaneIntersect(ScreenToWorldPointData screenToWorldPoint)
        {
            float _dist = (CenterPosition.z - screenToWorldPoint.Origin.z) / screenToWorldPoint.Direction.z;

            Vector3 hitPoint = screenToWorldPoint.Origin + screenToWorldPoint.Direction * _dist;

            return IsInside(hitPoint);
        }
        
        public Vector2Int IsInside(Vector3 position)
        {
            Vector2Int result = Vector2Int.zero;
            
            if (position.x > MaxPosition.x)
                result.x = 1;
            
            else if (position.x < MinPosition.x)
                result.x = -1;
            
            if (position.y > MaxPosition.y)
                result.y = 1;
            
            else if (position.y < MinPosition.y)
                result.y = -1;

            return result;
        }
        
        // public bool PlaneIntersect(ScreenToWorldPointData screenToWorldPoint)
        // {
        //     Vector3 p0 = MinPosition;
        //     Vector3 p1 = new Vector3(MinPosition.x, MaxPosition.y, CenterPosition.z);
        //     Vector3 p2 = MaxPosition;
        //     Vector3 p3 = new Vector3(MaxPosition.x, MinPosition.y, CenterPosition.z);
        //     
        //     if (Calculator.TriangleIntersect(p0, p1, p2, screenToWorldPoint))
        //         return true;
        //
        //     if (Calculator.TriangleIntersect(p1, p2, p3, screenToWorldPoint))
        //         return true;
        //
        //     if (Calculator.TriangleIntersect(p0, p3, (p1 + p2) * .5f, screenToWorldPoint))
        //         return true;
        //
        //     return Calculator.TriangleIntersect(p1, p2, (p0 + p3) * .5f, screenToWorldPoint);
        // }
    }
}