using System.Collections.Generic;
using UnityEngine;

namespace SplineRoad.BezierTrack.ScriptableObjects.Definitions
{
    [CreateAssetMenu]
    public class ScriptableMeshDataContainer : ScriptableObject
    {
        [SerializeField] private ScriptableShapeData _shapeData;
        [SerializeField] private Material _material;
        
        public ScriptableShapeData ShapeData => _shapeData;
        public List<Vertex> Vertices => ShapeData.ListOfVertices;
        public List<int> Lines => ShapeData.ListOfLines;
        public int VertexCount => ShapeData.VertexCount;
        public int LineCount => ShapeData.LineCount;
        public Material Material => _material;
    }
}
