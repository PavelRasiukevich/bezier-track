using System;
using UnityEngine;
using UnityEngine.Serialization;
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

        [HideInInspector] [Range(8, 32)] [SerializeField]
        private int _defaultSegmentResolution;

        [HideInInspector] [Range(0.1f, 256f)] [SerializeField]
        private float _tiling = 2f;

        //TODO: change range value to bigger values (.5f - 5f)
        [Range(1f, 16f)] [SerializeField] private float _roadWidth = 1f;
        [SerializeField] private TrackMode _mode;

        /// <summary>
        /// Test feature
        /// Changing to false leads to errors in some cases
        /// Do no change the value
        /// </summary>
        public bool AutoUpdate { get; private set; } = true;

        public float Tiling => _tiling;
        public Material Material => _meshDataContainer.Material;
        public SplineContainer SplineContainer => _splineContainer;
        public ScriptableMeshDataContainer MeshDataContainer => _meshDataContainer;
        public int SplinePointsCount => _splinePointsCount;

        public int DefaultSegmentResolution => _defaultSegmentResolution;
        public float RoadWidth => _roadWidth;
        public TrackMode Mode => _mode;
        public TrackMode LastMode { get; set; }

        private int _currentSplinePointsCount;

        private void OnValidate()
        {
            ValueChanged?.Invoke();

            if (_splineContainer == null)
            {
                _splineContainer = GetComponent<SplineContainer>();
            }

            if (_meshDataContainer == null)
            {
                _meshDataContainer = Resources.Load<ScriptableMeshDataContainer>("Stub Mesh Data Container");
            }
        }
    }
}