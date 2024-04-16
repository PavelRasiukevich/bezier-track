using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

namespace ptl.bezier
{
    public class TrackProperties : MonoBehaviour
    {
        private Action _valueChanged = () => { Debug.Log("Track Properties Empty"); };
        public event Action ValueChanged
        {
            add => _valueChanged += value;
            remove => _valueChanged -= value;
        }

        [Header("Data")] [SerializeField] private SplineContainer _splineContainer;

        [SerializeField] private ScriptableMeshDataContainer _meshDataContainer;

        [SerializeField] private int _splinePointsCount = 8;

        [HideInInspector] [Range(8, 32)] [SerializeField]
        private int _defaultSegmentResolution;

        [HideInInspector] [Range(0.1f, 256f)] [SerializeField]
        private float _tiling = 2f;

        [Range(1f, 16f)] [SerializeField] private float _roadWidth = 1f;
        [SerializeField] private TrackMode _mode;

        /// <summary>
        /// Test feature
        /// Changing to false leads to errors in some cases
        /// Do not change the value
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
            //_valueChanged?.Invoke();

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