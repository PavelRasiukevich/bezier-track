using System;
using System.Linq;
using Codice.Utils;
using ptl.bezier.editor;
using UnityEditor.Splines;
using UnityEngine;
using UnityEngine.Splines;

namespace ptl.bezier
{
    [RequireComponent(typeof(TrackProperties))]
    [RequireComponent(typeof(TrackCreator))]
    [RequireComponent(typeof(EditorSplineModifier))]
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
            _trackCreator.ClearTrack();
        }


        private void Delete()
        {
            _trackCreator = GetComponent<TrackCreator>();
            _trackCreator.DeleteTrackCompletely();
        }
    }
}