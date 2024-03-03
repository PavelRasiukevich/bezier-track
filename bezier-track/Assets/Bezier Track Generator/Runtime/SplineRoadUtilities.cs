using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace SplineRoad.BezierTrack
{
    public static class SplineRoadUtilities
    {
        public static Vector3 GetOrientedPoint(float3 tangent, float3 normal, float3 position, Vector3 point, float width = 1f)
        {
            var rot = Quaternion.LookRotation(tangent, normal);
            var pointRotation = rot * point * width;
            var pointLocation = (Vector3)position + pointRotation;
            
            return pointLocation;
        }

        public static Vector3 GetOrientedPoint(float t, SplineContainer container, Vector3 point, float width = 1f)
        {
            var rot = Quaternion.LookRotation(container.EvaluateTangent(t), container.EvaluateUpVector(t));
            var pointRotation = rot * point * width;
            var pointLocation = (Vector3)container.EvaluatePosition(t) + pointRotation;
            
            return pointLocation;
        }

        public static Vector3 GetVertexBaseOnKnot(BezierKnot knot, Vector3 point, float width = 1f)
        {
            var knotPosition = (Vector3)knot.Position;
            var knotRotation = (Quaternion)knot.Rotation;

            var pointRotation = knotRotation * point * width;
            var pointLocation = knotPosition + pointRotation;
            
            return pointLocation;
        }

        public static Vector3 LocalToWorldVec(float t, SplineContainer container, Vector3 point, float width = 1f)
        {
            return Quaternion.LookRotation(container.EvaluateTangent(t)) * point * width;
        }
    }
}