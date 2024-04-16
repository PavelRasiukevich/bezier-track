using System;
using UnityEditor;
using UnityEngine;

namespace ptl.bezier.editor
{
    [CustomEditor(typeof(TrackProperties))]
    public class TrackPropertiesCustomEditor : Editor
    {
        public event Action ValueChanged = delegate { };

        private int _splinePointsCount;
        private TrackProperties _target;

        private void OnEnable()
        {
            Debug.Log("TrackPropertiesCustomEditor");
            _target = target as TrackProperties;
            var splineModifier = _target.GetComponent<EditorSplineModifier>();
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            //getting value of Custom Editor
            _splinePointsCount = EditorGUILayout.IntSlider("Spline Points [8-128]", _splinePointsCount, 8, 128);
            //Setting values to the class the custom editor is for
            serializedObject.FindProperty("_splinePointsCount").intValue = _splinePointsCount;

            if (_splinePointsCount > 25)
            {
                Debug.Log(">25");
                ValueChanged.Invoke();
            }

            serializedObject.ApplyModifiedProperties();
            SaveChanges();
        }

        private void OnValidate()
        {
            Debug.Log("TrackPropertiesCustomEditor");
        }
    }
}