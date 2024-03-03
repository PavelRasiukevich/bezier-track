using System.Linq;
using UnityEditor;
using UnityEditor.Splines;
using UnityEngine;
using UnityEngine.Splines;

namespace SplineRoad.BezierTrack
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(TrackProperties))]
    [RequireComponent(typeof(MeshCreator))]
    public class RoadRoot : MonoBehaviour
    {
        private MeshCreator _meshCreator;
        private TrackProperties _trackProperties;

        private void Start()
        {
            Clear();
            Delete();
            Create();
        }

        private void OnValidate()
        {
            _trackProperties = GetComponent<TrackProperties>();
            _trackProperties.ValueChanged += OnValidateHandler;
            
            EditorSplineUtility.AfterSplineWasModified += AfterSplineWasModifiedHandler;
        }
        
        private void AfterSplineWasModifiedHandler(Spline arg1)
        {
            var cont = GetComponent<SplineContainer>();
            var splines = cont.Splines;

            var contains = splines.Contains(arg1);
            
            if (contains)
            {
                Clear();
                Create();
            }
        }

        private void OnValidateHandler()
        {
            Debug.Log("OnValidate");

            Clear();
            Create();
        }

        [ContextMenu("Create Mesh")]
        private void Create()
        {
            Debug.Log("Create");

            _meshCreator = GetComponent<MeshCreator>();
            _meshCreator.CreateMesh(GetComponent<TrackProperties>());
        }

        [ContextMenu("Clear Mesh Data")]
        private void Clear()
        {
            Debug.Log("Clear");

            _meshCreator = GetComponent<MeshCreator>();
            _meshCreator.ClearMesh();
        }

        [ContextMenu("Delete Completely")]
        private void Delete()
        {
            Debug.Log("Delete");

            _meshCreator = GetComponent<MeshCreator>();
            _meshCreator.DeleteCompletely();
        }

        private void OnDestroy()
        {
            Debug.Log("Destroyed");
            EditorSplineUtility.AfterSplineWasModified -= AfterSplineWasModifiedHandler;
            Delete();
        }
    }
}