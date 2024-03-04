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

        //TODO: change range value to bigger values (.5f - 5f)
        [Range(0.02f, .075f)] [SerializeField] private float _roadWidth;

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
                //find in project files stub 
                //add it to _meshDataContainer
            }
        }

        public void Draw(List<Vector3> v)
        {
            _v = v;
        }

        private void OnDrawGizmos()
        {
            // Debug.Log("DRAW");
            // Gizmos.color = Color.green;
            //
            // for (int i = 0; i < _v.Count; i++)
            // {
            //     Gizmos.DrawSphere(_v[i], 0.055f);
            // }
        }
    }
}