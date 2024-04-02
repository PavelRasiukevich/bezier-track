using System;
using UnityEngine;

namespace ptl.bezier
{
    public class KnotSegment : MonoBehaviour
    {
        [SerializeField] private int _id;
        [Range(8, 1024)] [SerializeField] private int _resolution = 8;

        public int Resolution
        {
            get => _resolution;
            set => _resolution = value;
        }

        private void OnValidate()
        {
            //OnValidate also works when script is loaded 
            //so it triggers when we add this component to a new created swgment object
            //KnotSegmentResolutionManager.SetValue(_id, KnotSegmentResolutionManager.GetValue(_id));
        }

        public void SetResolution(int value)
        {
        }
    }
}