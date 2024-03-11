using System;
using System.Collections.Generic;
using UnityEngine;

namespace ptl.bezier
{
    [Serializable]
    public class MeshConstructor
    {
        [SerializeField] private List<Vector3> _vertices;
        [SerializeField] private List<int> _triangles;
        [SerializeField] private List<Vector3> _normals;
        [SerializeField] private List<Vector2> _uvs;
        [SerializeField] private float _lenght;
        //[SerializeField] private int precision = 16;

        public MeshConstructor()
        {
            _vertices = new List<Vector3>();
            _triangles = new List<int>();
            _normals = new List<Vector3>();
            _uvs = new List<Vector2>();
        }

        public void ConstructMesh(TrackProperties properties, Mesh mesh)
        {
            LengthTable table = null;
            table = new LengthTable(properties.SplineContainer);

            _lenght = properties.SplineContainer.CalculateLength(0);

            //VERTICES 
            for (int i = 0; i < properties.SplinePointsCount; i++)
            {
                var t = i / (float)(properties.SplinePointsCount - 1);

                for (int j = 0; j < properties.MeshDataContainer.VertexCount; j++)
                {
                    var orientedPoint = SplineRoadUtilities.GetOrientedPointLocalSpace(
                        t,
                        properties.SplineContainer,
                        properties.MeshDataContainer.Vertices[j].Point,
                        properties.RoadWidth);

                    _vertices.Add(orientedPoint);
                }
            }

            //NORMALS 
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

            float tiling = properties.Material.mainTexture != null
                ? (float)properties.Material.mainTexture.width / properties.Material.mainTexture.height
                : 1f;

            float uSpan = properties.MeshDataContainer.ShapeData.USpan();

            tiling *= SplineRoadUtilities.GetArcLength(properties) / uSpan;
            tiling = Mathf.Max(1, Mathf.Round(tiling));


            //UVS 
            for (int i = 0; i < properties.SplinePointsCount; i++)
            {
                var t = i / (float)(properties.SplinePointsCount - 1);

                float tUv = t;
                tUv = table.ToPercentage(tUv);
                float uv0V = tUv * properties.Tiling;

                for (int j = 0; j < properties.MeshDataContainer.VertexCount; j++)
                {
                    _uvs.Add(new Vector2(properties.MeshDataContainer.Vertices[j].UVs.x, uv0V ));
                    //var u = properties.MeshDataContainer.Vertices[j].UVs.x;
                    //_uvs.Add(new Vector2(u, t * _lenght));
                }
            }

            //TRIANGLES 
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

        public void ClearMeshData()
        {
            _uvs.Clear();
            _triangles.Clear();
            _vertices.Clear();
            _normals.Clear();
        }
    }
}