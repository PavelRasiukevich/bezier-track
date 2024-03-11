using System;
using System.Collections.Generic;
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

        [Range(0.1f, 256f)] [SerializeField] 
        private float _tiling = 2f;
        
        //TODO: change range value to bigger values (.5f - 5f)
        [Range(1f, 3f)] [SerializeField] private float _roadWidth;

        public float Tiling => _tiling;
        public Material Material => _meshDataContainer.Material;
        public SplineContainer SplineContainer => _splineContainer;
        public ScriptableMeshDataContainer MeshDataContainer => _meshDataContainer;
        public int SplinePointsCount => _splinePointsCount;
        public float RoadWidth => _roadWidth;

        private List<Vector3> _v;

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