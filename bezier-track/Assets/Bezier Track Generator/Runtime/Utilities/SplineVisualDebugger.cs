using UnityEngine;
using UnityEngine.Splines;

namespace ptl.bezier
{
    public class SplineVisualDebugger : MonoBehaviour
    {
        [SerializeField] private SplineContainer _splineContainer;

        public bool AllTangents;

        [Range(0.01f, 1f)] public float KnotPointRadius;
        [Range(2, 64)] public float Resolution;

        private void OnDrawGizmos()
        {
            _splineContainer = GetComponent<SplineContainer>();

            DrawControlPoints();
            DrawEvaluatedTangents();
        }

        private void DrawControlPoints()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            var p0 = _splineContainer.Spline.GetCurve(0).P0;
            var p1 = _splineContainer.Spline.GetCurve(0).P1;
            var p2 = _splineContainer.Spline.GetCurve(0).P2;
            var p3 = _splineContainer.Spline.GetCurve(0).P3;

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(p0, KnotPointRadius);
            Gizmos.DrawSphere(p1, KnotPointRadius);
            Gizmos.DrawSphere(p2, KnotPointRadius);
            Gizmos.DrawSphere(p3, KnotPointRadius);
        }

        private void DrawEvaluatedTangents()
        {
            if (AllTangents)
            {
                for (int i = 0; i < Resolution; i++)
                {
                    var t = i / (Resolution - 1f);

                    var tangent = _splineContainer.Spline.EvaluateTangent(t);
                    var point = _splineContainer.Spline.EvaluatePosition(t);

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(point, tangent);
                }
            }
            
            Gizmos.color = Color.green;

            Vector3 t1 = (_splineContainer.Spline.GetCurve(0).P1 - _splineContainer.Spline.GetCurve(0).P0);
            Vector3 t2 = (_splineContainer.Spline.GetCurve(1).P2 - _splineContainer.Spline.GetCurve(0).P3);

            var r1 = (Vector3)_splineContainer.Spline.GetCurve(0).P0 + t1;
            var r2 = (Vector3)_splineContainer.Spline.GetCurve(0).P3 + t2;

            var pos1 = _splineContainer.Spline[0].Position;
            var pos2 = _splineContainer.Spline[1].Position;

            Gizmos.DrawSphere((Vector3)pos1 + t1, radius: KnotPointRadius);
            Gizmos.DrawSphere((Vector3)pos2 + t2, radius: KnotPointRadius);

            Gizmos.color = Color.magenta;

            Gizmos.DrawLine(pos1, (Vector3)pos1 + t1);
            Gizmos.DrawLine(pos2, (Vector3)pos2 + t2);
        }
    }
}