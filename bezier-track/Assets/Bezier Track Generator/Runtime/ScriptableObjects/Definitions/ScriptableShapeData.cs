using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

        public float USpan()
        {
            float distance = 0;

            for (int i = 0; i < LineCount; i += 2)
            {
                Vector3 a = Vertices[Lines[i]].Point;
                Vector3 b = Vertices[Lines[i + 1]].Point;
                distance += (a - b).magnitude;
            }

            return distance;
        }
    }

    [Serializable]
    public class Vertex
    {
        public Vector3 Point;
        public Vector3 Normal;
        public Vector2 UVs;
    }
}