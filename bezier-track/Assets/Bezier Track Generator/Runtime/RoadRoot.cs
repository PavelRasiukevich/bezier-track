using ptl.bezier.editor;
using UnityEngine;

namespace ptl.bezier
{
    [RequireComponent(typeof(TrackProperties))]
    [RequireComponent(typeof(TrackCreator))]
#if UNITY_EDITOR
    [RequireComponent(typeof(EditorSplineModifier))]
#endif
    public class RoadRoot : MonoBehaviour
    {
        private TrackCreator _trackCreator;
        private TrackProperties _trackProperties;
        private EditorSplineModifier _editorSplineModifier;

        private void Start()
        {
            Delete();
            Create();
        }

        private void Create()
        {
            _trackCreator = GetComponent<TrackCreator>();
            _trackCreator.CreateTrack(GetComponent<TrackProperties>());
        }

        private void Clear()
        {
            _trackCreator = GetComponent<TrackCreator>();
            _trackCreator.ClearTrack(GetComponent<TrackProperties>());
        }


        private void Delete()
        {
            _trackCreator = GetComponent<TrackCreator>();
            _trackCreator.DeleteTrackCompletely(GetComponent<TrackProperties>());
        }
    }
}