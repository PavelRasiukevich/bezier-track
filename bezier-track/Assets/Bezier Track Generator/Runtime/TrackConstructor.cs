using System;
using System.Collections.Generic;
using UnityEngine;

namespace ptl.bezier
{
    [Serializable]
    public class TrackConstructor
    {
        [SerializeField] private List<Vector3> _vertices;
        [SerializeField] private List<int> _triangles;
        [SerializeField] private List<Vector3> _normals;
        [SerializeField] private List<Vector2> _uvs;

        public void ConstructTrack(TrackProperties properties, Mesh mesh)
        {
            //VERTICES GENERATION
            _vertices = new List<Vector3>();

            for (int i = 0; i < properties.SplinePointsCount; i++)
            {
                var t = i / (float)(properties.SplinePointsCount - 1);
                
                for (int j = 0; j < properties.MeshDataContainer.VertexCount; j++)
                {
                    var orientedPoint = SplineRoadUtilities.GetOrientedPoint(
                        t,
                        properties.SplineContainer,
                        properties.MeshDataContainer.Vertices[j].point);

                    _vertices.Add(orientedPoint);
                }
            }

            //NORMALS GENERATION
            _normals = new List<Vector3>();

            for (int i = 0; i < properties.SplinePointsCount; i++)
            {
                var t = i / (float)(properties.SplinePointsCount - 1);

                for (int j = 0; j < properties.MeshDataContainer.VertexCount; j++)
                {
                    var point = SplineRoadUtilities.LocalToWorldVec(
                        t,
                        properties.SplineContainer,
                        properties.MeshDataContainer.Vertices[j].Normal);

                    _normals.Add(point);
                }
            }

            //UVS GENERATION
            _uvs = new List<Vector2>();
            var lentgh = properties.SplineContainer.CalculateLength(0);

            for (int i = 0; i < properties.SplinePointsCount; i++)
            {
                var t = i / (float)(properties.SplinePointsCount - 1);

                for (int j = 0; j < properties.MeshDataContainer.VertexCount; j++)
                {
                    var u = properties.MeshDataContainer.Vertices[j].UVs.x;
                    _uvs.Add(new Vector2(u, t * lentgh));
                }
            }

            //TRIANGLES GENERATION
            _triangles = new List<int>();

            for (int i = 0; i < properties.SplinePointsCount - 1; i++)
            {
                int rootIndex = (i) * properties.MeshDataContainer.VertexCount;
                int rootIndexNext = (i + 1) * properties.MeshDataContainer.VertexCount;

                for (int j = 0; j < properties.MeshDataContainer.LineCount; j += 2)
                {
                    int lineIndexA = properties.MeshDataContainer.Lines[j];
                    int lineIndexB = properties.MeshDataContainer.Lines[j + 1];

                    int currentA = rootIndex + lineIndexA;
                    int currentB = rootIndex + lineIndexB;

                    int nextA = rootIndexNext + lineIndexA;
                    int nextB = rootIndexNext + lineIndexB;

                    _triangles.Add(currentA);
                    _triangles.Add(nextA);
                    _triangles.Add(nextB);

                    _triangles.Add(currentA);
                    _triangles.Add(nextB);
                    _triangles.Add(currentB);
                }
            }

            mesh.SetVertices(_vertices);
            mesh.SetNormals(_normals);
            mesh.SetTriangles(_triangles, 0);
            mesh.SetUVs(0, _uvs);
        }
    }
}