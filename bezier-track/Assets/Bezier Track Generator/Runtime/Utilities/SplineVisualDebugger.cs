using UnityEngine;
using UnityEngine.Splines;

namespace ptl.bezier
{
    public class SplineVisualDebugger : MonoBehaviour
    {
        [SerializeField] private SplineContainer _splineContainer;
        [Range(0f, 1f)] [SerializeField] private float _t;

        public bool AllTangents;
        public int index;

        [Range(0.01f, 1f)] public float KnotPointRadius;
        [Range(2, 256)] public int Resolution;

        private Vector3 _tangent;
        private Vector3 _normal;
        private Vector3 _orientedPoint;

        private void OnDrawGizmos()
        {
            _splineContainer = GetComponent<SplineContainer>();

            // DrawControlPoints();
            //DrawEvaluatedTangents();

            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log(_splineContainer.Spline.Count);
            }

            var curve = _splineContainer.Spline.GetCurve(0);

            Gizmos.color = Color.yellow;

            //Gizmos.DrawSphere(DrawCurveByT(curve, _t), KnotPointRadius);
            //DrawCurveByT();

            // Gizmos.color = Color.blue;
            // Gizmos.DrawRay(Vector3.zero, _tangent);
            // Gizmos.color = Color.green;
            // Gizmos.DrawRay(Vector3.zero, _normal);


            //Gizmos.DrawSphere(_orientedPoint, KnotPointRadius / 2);

            //DrawCrossProduct();
            
            for (int j = 0; j < GetComponent<TrackCreator>().TrackConstructor.Verts.Count; j++)
            {
                Gizmos.color = Color.red;

                // _orientedPoint = SplineRoadUtilities.GetCurveOrientedPoint(
                //     properties.SplineContainer,
                //     properties.SplineContainer.Spline.GetCurve(i),
                //     properties.MeshDataContainer.Vertices[j].Point,
                //     tLocal,
                //     i,
                //     properties.RoadWidth
                // );

                Gizmos.DrawSphere(GetComponent<TrackCreator>().TrackConstructor.Verts[j], KnotPointRadius / 5);
            }
        }

        private void DrawCrossProduct()
        {
            Vector3 tangent = Vector3.forward;
            Vector3 arbitraryVector = new Vector3(1, 1, 0);
            Vector3 normalVector = Vector3.Cross(tangent, arbitraryVector);

            normalVector.Normalize();

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(Vector3.zero, tangent);
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(Vector3.zero, arbitraryVector);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(Vector3.zero, normalVector * 5);
        }

        private void DrawCurveByT()
        {
            //TODO: draw centered spher and tangent, normal, bi-normal

            var properties = GetComponent<TrackProperties>();

            for (int i = 0; i < properties.SplineContainer.Spline.Count - 1; i++)
            {
                var curve = properties.SplineContainer.Spline.GetCurve(i);

                for (int k = 0; k < Resolution; k++)
                {
                    var tLocal = k / (Resolution - 1f);

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(CurveUtility.EvaluatePosition(curve, tLocal), KnotPointRadius);

                    for (int j = 0; j < GetComponent<TrackCreator>().TrackConstructor.Verts.Count; j++)
                    {
                        Gizmos.color = Color.red;

                        // _orientedPoint = SplineRoadUtilities.GetCurveOrientedPoint(
                        //     properties.SplineContainer,
                        //     properties.SplineContainer.Spline.GetCurve(i),
                        //     properties.MeshDataContainer.Vertices[j].Point,
                        //     tLocal,
                        //     i,
                        //     properties.RoadWidth
                        // );

                        Gizmos.DrawSphere(GetComponent<TrackCreator>().TrackConstructor.Verts[j], KnotPointRadius / 5);
                    }
                }
            }
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