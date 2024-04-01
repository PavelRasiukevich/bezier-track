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

        public static Vector3 GetOrientedPointLocalSpace(Transform transform, float3 tangent, float3 normal, float3 position, Vector3 point, float width = 1f)
        {
            var rot = Quaternion.LookRotation(tangent, normal);
            var pointRotation = rot * point * width;
            var pointLocation = transform.InverseTransformPoint(position) + pointRotation;

            return pointLocation;
        }

        //TODO: refactor container, spline, curve
        public static Vector3 GetCurveOrientedPointLocalSpace(SplineContainer container, BezierCurve curve, Vector3 point, float t, float width = 1f)
        {
            var rot = Quaternion.LookRotation(CurveUtility.EvaluateTangent(curve, t), container.Spline.GetCurveUpVector(0, t));
            var pointRotation = rot * point * width;
            //var pointLocation = container.transform.InverseTransformPoint(CurveUtility.EvaluatePosition(curve, t)) + pointRotation;
            var pointLocation = (Vector3)CurveUtility.EvaluatePosition(curve, t) + pointRotation;

            return pointLocation;
        }

        public static Vector3 GetCurveOrientedPoint(SplineContainer container, BezierCurve curve, Vector3 point, float t, int index, float width = 1f)
        {
            var rot = Quaternion.LookRotation(CurveUtility.EvaluateTangent(curve, t), container.Spline.GetCurveUpVector(index, t));
            var pointRotation = rot * point * width;
            //var pointLocation = container.transform.InverseTransformPoint(CurveUtility.EvaluatePosition(curve, t)) + pointRotation;
            var pointLocation = (Vector3)CurveUtility.EvaluatePosition(curve, t) + pointRotation;

            return pointLocation;
        }

        public static Vector3 GetCurveOrientedPointLocalSpace(SplineContainer container, Vector3 point, float t, int index, float width = 1f)
        {
            var spline = container.Spline;
            var curve = spline.GetCurve(index);

            var position = CurveUtility.EvaluatePosition(curve, t);
            var tangent = CurveUtility.EvaluateTangent(curve, t);
            var normal = spline.GetCurveUpVector(index, t);

            var rot = Quaternion.LookRotation(tangent, normal);
            var pointRotation = rot * point * width;
            var pointLocation = (Vector3)position + pointRotation;

            return pointLocation;
        }

        public static Vector3 GetCurveOrientedPointLocalSpace(SplineContainer container, BezierCurve curve, Vector3 point, float t, int index, float width = 1f)
        {
            var spline = container.Spline;

            var position = CurveUtility.EvaluatePosition(curve, t);
            var tangent = CurveUtility.EvaluateTangent(curve, t);
            var normal = spline.GetCurveUpVector(index, t);

            var rot = Quaternion.LookRotation(tangent, normal);
            var pointRotation = rot * point * width;
            var pointLocation = (Vector3)position + pointRotation;

            return pointLocation;
        }

        public static Vector3 GetOrientedPointWorldSpace(float t, SplineContainer container, Vector3 point, float width = 1f)
        {
            var rot = Quaternion.LookRotation(container.EvaluateTangent(t), container.EvaluateUpVector(t));
            var pointRotation = rot * point * width;
            var pointLocation = (Vector3)container.EvaluatePosition(container.Spline, t) + pointRotation;
            //var pointLocation = (Vector3)container.EvaluatePosition(t) + pointRotation;

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

        public static Vector3 GetBezierCurvePoint(BezierCurve curve, float t)
        {
            var a = Vector3.Lerp(curve.P0, curve.P1, t);
            var b = Vector3.Lerp(curve.P1, curve.P2, t);
            var c = Vector3.Lerp(curve.P2, curve.P3, t);
            var d = Vector3.Lerp(a, b, t);
            var e = Vector3.Lerp(b, c, t);
            var point = Vector3.Lerp(d, e, t);

            return point;
        }

        public static Vector3 GetBezierCurveTangent(BezierCurve curve, float t)
        {
            var a = Vector3.Lerp(curve.P0, curve.P1, t);
            var b = Vector3.Lerp(curve.P1, curve.P2, t);
            var c = Vector3.Lerp(curve.P2, curve.P3, t);
            var d = Vector3.Lerp(a, b, t);
            var e = Vector3.Lerp(b, c, t);

            return (e - d).normalized;
        }

        public static Vector3 GetBezierCurveNormal(Vector3 tangent)
        {
            var rotation = Quaternion.AngleAxis(90, Vector3.forward);
            var normal = rotation * tangent;

            return normal;
        }
    }
}