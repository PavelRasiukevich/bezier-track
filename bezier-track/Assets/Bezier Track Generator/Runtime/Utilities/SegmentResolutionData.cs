using System;
using System.Collections.Generic;

namespace ptl.bezier
{
    [Serializable]
    public class SegmentResolutionData
    {
        public List<int> Values;

        public SegmentResolutionData()
        {
            Values = new List<int>();
        }
    }
}