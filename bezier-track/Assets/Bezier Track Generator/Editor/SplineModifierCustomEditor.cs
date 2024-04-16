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
        private TrackPropertiesCustomEditor[] _propertiesCustomEditor;

        private void OnEnable()
        {
            _trackCreator = serializedObject.FindProperty("_trackCreator").objectReferenceValue as TrackCreator;
            _trackProperty = serializedObject.FindProperty("_trackProperties").objectReferenceValue as TrackProperties;

            _propertiesCustomEditor = Resources.FindObjectsOfTypeAll<TrackPropertiesCustomEditor>();

            foreach (var item in _propertiesCustomEditor)
            {
                item.ValueChanged += ValueChangedHandler;
            }
        }

        private void OnDisable()
        {
            foreach (var item in _propertiesCustomEditor)
            {
                item.ValueChanged -= ValueChangedHandler;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            HandleButtons();
        }

        private void ValueChangedHandler()
        {
            Debug.Log("VALUE CHANGED HANDLER EXECUTED");
        }

        private void HandleButtons()
        {
            var createHasBeenClicked = GUILayout.Button("Create");
            var clearHasBeenClicked = GUILayout.Button("Clear");
            var deleteHasBeenClicked = GUILayout.Button("Delete");

            if (createHasBeenClicked)
            {
                _trackCreator.CreateTrack(_trackProperty);
            }
            else if (clearHasBeenClicked)
            {
            }
            else if (deleteHasBeenClicked)
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