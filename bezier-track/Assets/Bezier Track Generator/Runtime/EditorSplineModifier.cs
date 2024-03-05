using System.Linq;
using UnityEditor.Splines;
using UnityEngine;
using UnityEngine.Splines;

namespace ptl.bezier.editor
{
    [ExecuteInEditMode]
    public class EditorSplineModifier : MonoBehaviour
    {
        private TrackCreator _trackCreator;
        private TrackProperties _trackProperties;
        private SplineContainer _splineContainer;

        private void OnEnable()
        {
            if (!Application.isEditor) return;

            Init();
            InitialBuild();

            _trackProperties.ValueChanged += PropertiesValueChangeHandler;
            EditorSplineUtility.AfterSplineWasModified += ModifiedHandler;
        }

        private void OnDisable()
        {
            if (!Application.isEditor) return;

            _trackProperties.ValueChanged -= PropertiesValueChangeHandler;
            EditorSplineUtility.AfterSplineWasModified -= ModifiedHandler;
        }

        private void OnDestroy()
        {
            if (!Application.isEditor) return;

            _trackProperties.ValueChanged -= PropertiesValueChangeHandler;
            EditorSplineUtility.AfterSplineWasModified -= ModifiedHandler;
        }

        private void PropertiesValueChangeHandler()
        {
            Clear();
            Create();
        }

        private void ModifiedHandler(Spline spline)
        {
            if (_splineContainer.Splines.Contains(spline))
            {
                Clear();
                Create();
            }
        }

        private void Init()
        {
            _trackCreator = GetComponent<TrackCreator>();
            _trackProperties = GetComponent<TrackProperties>();
            _splineContainer = GetComponent<SplineContainer>();
        }

        private void InitialBuild()
        {
            _trackCreator.DeleteTrackCompletely();
            _trackCreator.CreateTrack(_trackProperties);
        }

        [ContextMenu("Create Track")]
        private void Create() => _trackCreator.CreateTrack(_trackProperties);

        [ContextMenu("Clear Track Data")]
        private void Clear() => _trackCreator.ClearTrack();

        [ContextMenu("Delete Completely")]
        private void Delete() => _trackCreator.DeleteTrackCompletely();
    }
}