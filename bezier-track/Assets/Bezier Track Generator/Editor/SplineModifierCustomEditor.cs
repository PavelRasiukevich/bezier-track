using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

namespace ptl.bezier.editor
{
    [CustomEditor(typeof(EditorSplineModifier))]
    public class SplineModifierCustomEditor : Editor
    {
        private TrackCreator _trackCreator;
        private TrackProperties _trackProperty;

        private void OnEnable()
        {
            _trackCreator = serializedObject.FindProperty("_trackCreator").objectReferenceValue as TrackCreator;
            _trackProperty = serializedObject.FindProperty("_trackProperties").objectReferenceValue as TrackProperties;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            HandleButtons();
        }

        private void HandleButtons()
        {
            var createHasBeenClocked = GUILayout.Button("Create");
            var clearHasBeenClocked = GUILayout.Button("Clear");
            var deleteHasBeenClocked = GUILayout.Button("Delete");

            if (createHasBeenClocked)
            {
                _trackCreator.CreateTrack(_trackProperty);
            }
            else if (clearHasBeenClocked)
            {
            }
            else if (deleteHasBeenClocked)
            {
                _trackCreator.DeleteTrackCompletely(_trackProperty);
            }
            else
            {
                return;
            }
        }
    }
}