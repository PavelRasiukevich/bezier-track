using System;
using System.Collections.Generic;
using UnityEngine;

namespace ptl.bezier
{
    [CreateAssetMenu(menuName = "Create Shape Data Asset", fileName = "Shape Data")]
    public class ScriptableShapeData : ScriptableObject
    {
        public List<Vertex> Vertices;
        public List<int> Lines;
        
        public List<Vertex> ListOfVertices => Vertices;
        public List<int> ListOfLines => Lines;
        public int VertexCount => Vertices.Count;
        public int LineCount => Lines.Count;
        
    }

    [Serializable]
    public class Vertex
    {
        public Vector3 point;
        public Vector3 Normal;
        public Vector2 UVs;
    }
}