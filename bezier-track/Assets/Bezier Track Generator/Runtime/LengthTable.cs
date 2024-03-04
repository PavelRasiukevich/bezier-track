using UnityEngine;
using UnityEngine.Splines;

namespace ptl.bezier
{
    public class LengthTable {

        public float[] distances;

        int SmpCount => distances.Length;
        float TotalLength => distances[SmpCount - 1];
        public LengthTable( SplineContainer container, int precision = 16 ) {
            
            distances = new float[precision];
            Vector3 prevPoint = container.Spline.GetCurve(0).P0;
            
            distances[0] = 0f;
            for( int i = 1; i < precision; i++ ) {
                float t = i / (precision - 1f);
                Vector3 currentPoint = container.Spline.EvaluatePosition(t);
                float delta = (prevPoint-currentPoint).magnitude;
                distances[i] = distances[i - 1] + delta;
                prevPoint = currentPoint;
            }
        }

        // Convert the t-value to percentage of distance along the curve
        public float ToPercentage( float t ) {
            float iFloat = t * (SmpCount-1);
            int idLower = Mathf.FloorToInt(iFloat);
            int idUpper = Mathf.FloorToInt(iFloat + 1);
            if( idUpper >= SmpCount ) idUpper = SmpCount - 1;
            if( idLower < 0 ) idLower = 0;
            return Mathf.Lerp( distances[idLower], distances[idUpper], iFloat - idLower ) / TotalLength;
        }
   
    }
}