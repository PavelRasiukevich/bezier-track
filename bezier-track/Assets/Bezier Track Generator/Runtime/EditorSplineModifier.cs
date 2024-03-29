using System.Linq;
#if UNITY_EDITOR
using UnityEditor.Splines;
#endif
using UnityEngine;
using UnityEngine.Splines;

namespace ptl.bezier.editor
{
#if UNITY_EDITOR
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
            EditorSplineUtility.AfterSplineWasModified += SplineModifiedHandler;
        }

        private void OnDisable()
        {
            if (!Application.isEditor) return;

            _trackProperties.ValueChanged -= PropertiesValueChangeHandler;
            EditorSplineUtility.AfterSplineWasModified -= SplineModifiedHandler;
        }

        private void OnDestroy()
        {
            if (!Application.isEditor) return;

            _trackProperties.ValueChanged -= PropertiesValueChangeHandler;
            EditorSplineUtility.AfterSplineWasModified -= SplineModifiedHandler;
        }

        private void PropertiesValueChangeHandler()
        {
            if (_trackProperties.Mode == TrackMode.Single && _trackProperties.LastMode == TrackMode.Single)
            {
                Clear();
            }
            else
            {
                _trackCreator.DeleteOnValidate(_trackProperties);
            }

            Create();
        }

        private void SplineModifiedHandler(Spline spline)
        {
            if (_splineContainer.Splines.Contains(spline))
            {
                Delete();
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
            _trackCreator.DeleteTrackCompletely(_trackProperties);
            _trackCreator.CreateTrack(_trackProperties);
        }

        [ContextMenu("Create Track")]
        private void Create() => _trackCreator.CreateTrack(_trackProperties);

        [ContextMenu("Clear Track Data")]
        private void Clear() => _trackCreator.ClearTrack(_trackProperties);

        [ContextMenu("Delete Completely")]
        private void Delete() => _trackCreator.DeleteTrackCompletely(_trackProperties);
    }
#endif
}