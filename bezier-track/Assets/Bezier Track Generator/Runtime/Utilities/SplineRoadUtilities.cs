using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace ptl.bezier
{
    public static class SplineRoadUtilities
    {
        public static Vector3 GetOrientedPointWorldSpace(float3 tangent, float3 normal, float3 position, Vector3 point, float width = 1f)
        {
            var rot = Quaternion.LookRotation(tangent, normal);
            var pointRotation = rot * point * width;
            var pointLocation = (Vector3)position + pointRotation;

            return pointLocation;
        }

        public static Vector3 GetOrientedPointWorldSpace(float t, SplineContainer container, Vector3 point, float width = 1f)
        {
            var rot = Quaternion.LookRotation(container.EvaluateTangent(t), container.EvaluateUpVector(t));
            var pointRotation = rot * point * width;
            var pointLocation = (Vector3)container.EvaluatePosition(t) + pointRotation;

            return pointLocation;
        }

        public static Vector3 GetOrientedPointLocalSpace(float t, SplineContainer container, Vector3 point, float width = 1f)
        {
            var rot = Quaternion.LookRotation(container.EvaluateTangent(t), container.EvaluateUpVector(t));
            var pointRotation = rot * point * width;
            var pointLocation = container.transform.InverseTransformPoint(container.EvaluatePosition(t)) + pointRotation;

            return pointLocation;
        }

        //TODO: delete if there are no uses
        public static Vector3 GetOrientedPointOnSpline(float t, SplineContainer container, Vector3 point, float width = 1f)
        {
            var spline = container.Spline;

            var rot = Quaternion.LookRotation(spline.EvaluateTangent(t), spline.EvaluateUpVector(t));
            var pointRotation = rot * point * width;
            var pointLocation = (Vector3)spline.EvaluatePosition(t) + pointRotation;

            return pointLocation;
        }

        public static Vector3 GetVertexBasedOnKnot(BezierKnot knot, Vector3 point, float width = 1f)
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

        public static float GetArcLength(TrackProperties properties, int precision = 16)
        {
            Vector3[] points = new Vector3[precision];
            for (int i = 0; i < precision; i++)
            {
                float t = i / (precision - 1f);
                points[i] = properties.SplineContainer.Spline.EvaluatePosition(t);
            }

            float dist = 0;
            for (int i = 0; i < precision - 1; i++)
            {
                Vector3 a = points[i];
                Vector3 b = points[i + 1];
                dist += Vector3.Distance(a, b);
            }

            return dist;
        }
    }
}