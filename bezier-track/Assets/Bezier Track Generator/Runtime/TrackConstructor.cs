using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace ptl.bezier
{
    [Serializable]
    public class TrackConstructor
    {
        [SerializeField] private List<Vector3> _vertices;
        [SerializeField] private List<int> _triangles;
        [SerializeField] private List<Vector3> _normals;
        [SerializeField] private List<Vector2> _uvs;
        [SerializeField] private float _lenght;

        public List<Vector3> Verts => _vertices;

        //[SerializeField] private int precision = 16;
        // private Vector3 _current;
        // private Vector3 _previous;
        // private float _distance;

        public TrackConstructor()
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
                var t = i / (properties.SplinePointsCount - 1f);

                float tUv = t;
                tUv = table.ToPercentage(tUv);
                float uv0V = tUv * _lenght / uSpan;

                for (int j = 0; j < properties.MeshDataContainer.VertexCount; j++)
                {
                    _uvs.Add(new Vector2(properties.MeshDataContainer.Vertices[j].UVs.x, uv0V));
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

        public void ConstructBezierCurveVertices(TrackProperties properties, Mesh mesh, int curveIndex)
        {
            for (int i = 0; i < properties.SplinePointsCount; i++)
            {
                var t = i / (properties.SplinePointsCount - 1f);

                for (int j = 0; j < properties.MeshDataContainer.VertexCount; j++)
                {
                    var orientedPoint = SplineRoadUtilities.GetCurveOrientedPointLocalSpace(
                        properties.SplineContainer,
                        properties.MeshDataContainer.Vertices[j].Point,
                        t,
                        curveIndex,
                        properties.RoadWidth
                    );

                    _vertices.Add(orientedPoint);
                }
            }

            mesh.SetVertices(_vertices);
        }

        public void ConstructBezierCurveNormals(TrackProperties properties, Mesh mesh)
        {
            for (int i = 0; i < properties.SplinePointsCount; i++)
            {
                var t = i / (properties.SplinePointsCount - 1f);

                for (int j = 0; j < properties.MeshDataContainer.VertexCount; j++)
                {
                    var point = SplineRoadUtilities.LocalToWorldVec(
                        t,
                        properties.SplineContainer,
                        properties.MeshDataContainer.Vertices[j].Normal);

                    _normals.Add(point);
                }
            }

            mesh.SetNormals(_normals);
        }

        public void ConstructBezierUVs(TrackProperties properties, Mesh mesh)
        {
            LengthTable table = null;
            table = new LengthTable(properties.SplineContainer);

            _lenght = properties.SplineContainer.CalculateLength(0);

            float tiling = properties.Material.mainTexture != null
                ? (float)properties.Material.mainTexture.width / properties.Material.mainTexture.height
                : 1f;

            float uSpan = properties.MeshDataContainer.ShapeData.USpan();

            tiling *= SplineRoadUtilities.GetArcLength(properties) / uSpan;
            tiling = Mathf.Max(1, Mathf.Round(tiling));


            for (int i = 0; i < properties.SplinePointsCount; i++)
            {
                var t = i / (properties.SplinePointsCount - 1f);

                float tUv = t;
                tUv = table.ToPercentage(tUv);
                float uv0V = tUv * _lenght / uSpan;

                for (int j = 0; j < properties.MeshDataContainer.VertexCount; j++)
                {
                    _uvs.Add(new Vector2(properties.MeshDataContainer.Vertices[j].UVs.x,
                        uv0V / properties.SplineContainer.Spline.Count));
                }
            }

            mesh.SetUVs(0, _uvs);
        }

        /// <summary>
        /// Use ConstructTriangles Instead
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="mesh"></param>
        /// 
        [Obsolete]
        public void ConstructBezierCurveTriangles(TrackProperties properties, Mesh mesh)
        {
            for (int i = 0; i < properties.SplinePointsCount - 1; i++)
            {
                int rootIndex = i * properties.MeshDataContainer.VertexCount; // 0
                int rootIndexNext = (i + 1) * properties.MeshDataContainer.VertexCount; // 16

                for (int j = 0; j < properties.MeshDataContainer.LineCount; j += 2)
                {
                    int lineIndexA = properties.MeshDataContainer.Lines[j]; // 15
                    int lineIndexB = properties.MeshDataContainer.Lines[j + 1]; // 0

                    int currentA = rootIndex + lineIndexA; // 0 + 15 = 15
                    int currentB = rootIndex + lineIndexB; // 0 + 0 = 0

                    int nextA = rootIndexNext + lineIndexA; // 17 + 15 = 31
                    int nextB = rootIndexNext + lineIndexB; // 17 + 0 = 16

                    _triangles.Add(currentA);
                    _triangles.Add(nextA);
                    _triangles.Add(nextB);

                    _triangles.Add(currentA);
                    _triangles.Add(nextB);
                    _triangles.Add(currentB);
                }
            }

            mesh.SetTriangles(_triangles, 0);
        }

        public void ConstructVertices(TrackProperties properties, Mesh mesh, int k)
        {
            var t = k / (float)(properties.SplinePointsCount - 1);

            for (int j = 0; j < properties.MeshDataContainer.VertexCount; j++)
            {
                var orientedPoint = SplineRoadUtilities.GetOrientedPointLocalSpace(
                    t,
                    properties.SplineContainer,
                    properties.MeshDataContainer.Vertices[j].Point,
                    properties.RoadWidth);

                _vertices.Add(orientedPoint);
            }

            mesh.SetVertices(_vertices);
        }

        public void ConstructNormals(TrackProperties properties, Mesh mesh, int k)
        {
            var t = k / (float)(properties.SplinePointsCount - 1);

            for (int j = 0; j < properties.MeshDataContainer.VertexCount; j++)
            {
                var point = SplineRoadUtilities.LocalToWorldVec(
                    t,
                    properties.SplineContainer,
                    properties.MeshDataContainer.Vertices[j].Normal);

                _normals.Add(point);
            }

            mesh.SetNormals(_normals);
        }

        public void ConstructUVs(TrackProperties properties, Mesh mesh, int k)
        {
            LengthTable table = null;
            table = new LengthTable(properties.SplineContainer);

            _lenght = properties.SplineContainer.CalculateLength(0);

            float tiling = properties.Material.mainTexture != null
                ? (float)properties.Material.mainTexture.width / properties.Material.mainTexture.height
                : 1f;

            float uSpan = properties.MeshDataContainer.ShapeData.USpan();

            tiling *= SplineRoadUtilities.GetArcLength(properties) / uSpan;
            tiling = Mathf.Max(1, Mathf.Round(tiling));

            var t = k / (properties.SplinePointsCount - 1f);

            float tUv = t;
            tUv = table.ToPercentage(tUv);
            float uv0V = tUv * _lenght / uSpan;

            for (int j = 0; j < properties.MeshDataContainer.VertexCount; j++)
            {
                _uvs.Add(new Vector2(properties.MeshDataContainer.Vertices[j].UVs.x, uv0V));
            }

            mesh.SetUVs(0, _uvs);
        }

        public void ConstructTriangles(TrackProperties properties, Mesh mesh, int value)
        {
            for (int i = 0; i < value; i++)
            {
                int rootIndex = i * properties.MeshDataContainer.VertexCount;
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

            mesh.SetTriangles(_triangles, 0);
        }

        public void ClearMeshData()
        {
            _uvs.Clear();
            _triangles.Clear();
            _vertices.Clear();
            _normals.Clear();
        }

        public void ClearTriangles()
        {
            _triangles.Clear();
        }

        public void ClearUVs()
        {
            _uvs.Clear();
        }
    }
}