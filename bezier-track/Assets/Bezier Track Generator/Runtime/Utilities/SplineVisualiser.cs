using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace ptl.bezier
{
    public class SplineVisualiser : MonoBehaviour
    {
        [SerializeField] private TrackProperties _properties;

        [SerializeField] private float _splinePointRadius;
        [SerializeField] private Color _splinePointColor;

        [SerializeField] private float _vertexVisualPointRadius;
        [SerializeField] private Color _vertexVisualPointColor;

        [SerializeField] private bool _testGizmosEnable;
        [SerializeField] private bool _vertexGizmosDrawEnable;
        [SerializeField] private bool _vectorVisualsEnabled;
        [SerializeField] private bool _handlesEnabled;
        [SerializeField] private bool _drawOnKnot;
        [SerializeField] private bool _drawVertex;

        [SerializeField] private float _magnitude;

        private Color _default = Color.white;

        private void OnDrawGizmos()
        {
            if (_drawVertex)
            {
                DrawVertices();
            }

            if (_testGizmosEnable)
            {
            }

            if (_vertexGizmosDrawEnable)
            {
                //DrawVertices();
                //DrawLinesBetweenVertices();
                DrawLinesBetweenVerticesUsingPairs();
            }

            if (_drawOnKnot)
            {
                DrawVertexBasedOnKnot();
            }
        }
        
        private void DrawSplinePoints()
        {
            for (int i = 0; i < _properties.SplinePointsCount; i++)
            {
                var t = i / (float)(_properties.SplinePointsCount - 1);
                _properties.SplineContainer.Evaluate(0, t, out float3 position, out float3 tangent, out float3 normal);

                Gizmos.color = _splinePointColor;
                Gizmos.DrawSphere(position, _splinePointRadius);

                if (_handlesEnabled)
                    Handles.PositionHandle(Handles.PositionHandleIds.@default, position, Quaternion.LookRotation(tangent, Vector3.up));

                Vector3 biNormalVector = Vector3.Cross(normal, tangent).normalized;

                if (_vectorVisualsEnabled)
                {
                    //draw rays instead of handles
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(position, biNormalVector.normalized * _magnitude);
                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(position, normal * _magnitude);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawRay(position, (tangent * _magnitude));
                    //
                }

                float3 rigthPoint = (Vector3)position + (-biNormalVector * _properties.RoadWidth);
                float3 leftPoint = (Vector3)position + (biNormalVector * _properties.RoadWidth);

                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(rigthPoint, _vertexVisualPointRadius);
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(leftPoint, _vertexVisualPointRadius);
            }
        }

        private void DrawVertices()
        {
            for (int i = 0; i < _properties.SplinePointsCount; i++)
            {
                var t = i / (float)(_properties.SplinePointsCount - 1);

                Gizmos.color = _vertexVisualPointColor;

                for (int j = 0; j < _properties.MeshDataContainer.VertexCount; j++)
                {
                    var orientedPoint = SplineRoadUtilities.GetOrientedPointLocalSpace(
                        t,
                        _properties.SplineContainer,
                        _properties.MeshDataContainer.Vertices[j].Point ,1f);

                    Gizmos.DrawSphere(orientedPoint, _vertexVisualPointRadius);
                }
            }
        }

        private void DrawLinesBetweenVertices()
        {
            for (int i = 0; i < _properties.SplinePointsCount; i++)
            {
                Gizmos.color = _default;
                var t = i / (float)(_properties.SplinePointsCount - 1);

                for (int j = 0; j < _properties.MeshDataContainer.VertexCount; j += 2)
                {
                    var current = SplineRoadUtilities.GetOrientedPointWorldSpace(
                        t,
                        _properties.SplineContainer,
                        _properties.MeshDataContainer.Vertices[j + 1].Point,
                        _properties.RoadWidth);


                    var next = SplineRoadUtilities.GetOrientedPointWorldSpace(
                        t,
                        _properties.SplineContainer,
                        _properties.MeshDataContainer.Vertices[(j + 2) % _properties.MeshDataContainer.Vertices.Count].Point,
                        _properties.RoadWidth);

                    Gizmos.DrawLine(current, next);
                }
            }
        }

        //just another implementation for already existing functionality
        private void DrawLinesBetweenVerticesUsingPairs()
        {
            for (int i = 0; i < _properties.SplinePointsCount; i++)
            {
                Gizmos.color = _default;
                var t = i / (float)(_properties.SplinePointsCount - 1);

                for (int j = 0; j < _properties.MeshDataContainer.VertexCount; j += 2)
                {
                    var current = SplineRoadUtilities.GetOrientedPointWorldSpace(
                        t,
                        _properties.SplineContainer,
                        _properties.MeshDataContainer.Vertices[_properties.MeshDataContainer.Lines[j]].Point);


                    var next = SplineRoadUtilities.GetOrientedPointWorldSpace(
                        t,
                        _properties.SplineContainer,
                        _properties.MeshDataContainer.Vertices[(_properties.MeshDataContainer.Lines[j + 1]) % _properties.MeshDataContainer.Vertices.Count].Point);

                    Gizmos.DrawLine(current, next);
                }
            }
        }

        /// <summary>
        /// Draws vertex using bezier knot from spline as reference for our points
        /// </summary>
        private void DrawVertexBasedOnKnot()
        {
            for (int i = 0; i < _properties.SplineContainer.Spline.Knots.Count(); i++)
            {
                Gizmos.color = _splinePointColor;
                
                for (int j = 0; j < _properties.MeshDataContainer.VertexCount; j++)
                {
                    var orientedPoint = SplineRoadUtilities.GetVertexBasedOnKnot(
                        _properties.SplineContainer.Spline.Knots.ToArray()[i],
                        _properties.MeshDataContainer.Vertices[j].Point
                    );

                    Gizmos.DrawSphere(orientedPoint, _splinePointRadius);
                }
            }
        }
    }
}