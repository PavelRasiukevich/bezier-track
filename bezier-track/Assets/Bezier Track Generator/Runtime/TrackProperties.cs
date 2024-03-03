using System;
using UnityEngine;
using UnityEngine.Splines;

namespace ptl.bezier
{
    public class TrackProperties : MonoBehaviour
    {
        public event Action ValueChanged = delegate { };

        [Header("Data")] [SerializeField] private SplineContainer _splineContainer;
        [SerializeField] private ScriptableMeshDataContainer _meshDataContainer;

        [Header("Settings")] [Range(8, 1024)] [SerializeField]
        private int _splinePointsCount = 8;

        //TODO: change range value to bigger values (.5f - 5f)
        [Range(0.02f, .075f)] [SerializeField] private float _roadWidth;

        public Material Material => _meshDataContainer.Material;
        public SplineContainer SplineContainer => _splineContainer;
        public ScriptableMeshDataContainer MeshDataContainer => _meshDataContainer;
        public int SplinePointsCount => _splinePointsCount;
        public float RoadWidth => _roadWidth;

        private void OnValidate()
        {
            ValueChanged?.Invoke();

            if (_splineContainer == null)
            {
                _splineContainer = GetComponent<SplineContainer>();
            }

            if (_meshDataContainer == null)
            {
                //find in project files stub 
                //add it to _meshDataContainer
            }
        }
    }
}