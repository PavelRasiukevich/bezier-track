using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompensatedBezierUV : MonoBehaviour
{
     public Transform[] controlPoints; // Bezier curve control points
    public int segments = 100; // Number of segments for length calculation
    public Material material; // The material applied to the mesh

    private Mesh mesh;
    private float[] cumulativeLengths;

    private void Start()
    {
        GenerateBezierCurveMesh();
    }

    private void GenerateBezierCurveMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CalculateCumulativeLengths();

        Vector3[] vertices = new Vector3[(segments + 1) * 2];
        Vector2[] uv = new Vector2[(segments + 1) * 2];

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 point = Bezier(controlPoints[0].position, controlPoints[1].position, controlPoints[2].position, controlPoints[3].position, t);

            vertices[i] = point;
            vertices[i + segments + 1] = point;

            float normalizedLength = CalculateNormalizedLength(t);
            uv[i] = new Vector2(normalizedLength, 0f); // UV coordinate for the bottom part of the mesh
            uv[i + segments + 1] = new Vector2(normalizedLength, 1f); // UV coordinate for the top part of the mesh
        }

        int[] triangles = new int[segments * 6];

        for (int i = 0, ti = 0; i < segments; i++, ti += 6)
        {
            triangles[ti] = i;
            triangles[ti + 1] = i + segments + 1;
            triangles[ti + 2] = i + 1;

            triangles[ti + 3] = i + 1;
            triangles[ti + 4] = i + segments + 1;
            triangles[ti + 5] = i + segments + 2;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        if (material != null)
        {
            GetComponent<MeshRenderer>().material = material;
        }
    }

    private void CalculateCumulativeLengths()
    {
        cumulativeLengths = new float[segments + 1];
        float totalLength = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 currentPoint = Bezier(controlPoints[0].position, controlPoints[1].position, controlPoints[2].position, controlPoints[3].position, t);

            if (i > 0)
            {
                float segmentLength = Vector3.Distance(currentPoint,
                    Bezier(controlPoints[0].position, controlPoints[1].position, controlPoints[2].position, controlPoints[3].position, (i - 1) / (float)segments));
                totalLength += segmentLength;
                cumulativeLengths[i] = totalLength;
            }
        }
    }

    private float CalculateNormalizedLength(float t)
    {
        float targetLength = t * cumulativeLengths[segments];
        for (int i = 0; i <= segments; i++)
        {
            if (cumulativeLengths[i] >= targetLength)
            {
                if (i == 0)
                    return 0f;
                float segmentStart = cumulativeLengths[i - 1];
                float segmentEnd = cumulativeLengths[i];
                float segmentT = Mathf.InverseLerp(segmentStart, segmentEnd, targetLength);
                return (i - 1 + segmentT) / segments;
            }
        }
        return 1f;
    }

    private Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}
