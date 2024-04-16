using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ptl.bezier
{
    public class TrackCreator : MonoBehaviour
    {
        [HideInInspector] [SerializeField] private Mesh _mesh;
        [HideInInspector] [SerializeField] private GameObject _track;
        [SerializeField] private List<Mesh> _meshes;
        [SerializeField] private List<KnotSegment> _tracks;
        [SerializeField] private TrackConstructor _trackConstructor;

        public TrackConstructor TrackConstructor => _trackConstructor;

        private SegmentResolutionData _segmentResolutionData;

        public void CreateTrack(TrackProperties properties)
        {
            switch (properties.Mode)
            {
                case TrackMode.Single:
                    CreateSingleTrack(properties);
                    properties.LastMode = TrackMode.Single;
                    break;
                case TrackMode.KnotBased:
                    CreateTrackBasedOnKnots(properties);
                    properties.LastMode = TrackMode.KnotBased;
                    break;
                default:
                    Debug.Log("Not Implemented");
                    break;
            }
        }

        private void CreateSingleTrack(TrackProperties properties)
        {
            if (_mesh == null)
            {
                _mesh = new Mesh
                {
                    name = "Procedural Track"
                };
            }

            if (_track == null)
            {
                _track = new GameObject("Track");

                _track.AddComponent<MeshFilter>();
                _track.AddComponent<MeshRenderer>();
                _track.AddComponent<MeshCollider>().sharedMesh = _mesh;

                _track.transform.parent = properties.transform;
                _track.transform.position = properties.transform.position;
                _track.transform.rotation = properties.transform.rotation;
            }

            _track.GetComponent<MeshFilter>().sharedMesh = _mesh;
            _track.GetComponent<MeshRenderer>().sharedMaterial = properties.Material;

            _trackConstructor ??= new TrackConstructor();
            _trackConstructor.ConstructMesh(properties, _mesh);
        }

        private void CreateMultipleTrack(TrackProperties properties)
        {
            /*var segments = properties.SplinePointsCount - 1;

            for (int i = 0; i < segments; i++)
            {
                var mesh = new Mesh
                {
                    name = "Procedural Mesh " + "_" + i
                };

                var track = new GameObject($"Track Segment _{i}");

                _meshes.Add(mesh);
                _tracks.Add(track);

                track.AddComponent<MeshFilter>();
                track.AddComponent<MeshRenderer>();

                track.transform.parent = properties.transform;
                track.transform.position = properties.transform.position;
                track.transform.rotation = properties.transform.rotation;

                track.GetComponent<MeshFilter>().sharedMesh = mesh;
                track.GetComponent<MeshRenderer>().sharedMaterial = properties.Material;

                _trackConstructor ??= new TrackConstructor();

                for (int j = 0; j < properties.DefaultSegmentResolution; j++)
                {
                    _trackConstructor.ConstructVertices(properties, mesh, j + i);
                    _trackConstructor.ConstructNormals(properties, mesh, j + i);
                    _trackConstructor.ConstructUVs(properties, mesh, j + i);
                }

                _trackConstructor.ConstructTriangles(properties, mesh, 1);
                _trackConstructor.ClearMeshData();
            }*/
        }

        private void CreateTrackBasedOnKnots(TrackProperties properties)
        {
            _meshes = new List<Mesh>();
            _tracks = new List<KnotSegment>();

            for (int i = 0; i < properties.SplineContainer.Spline.Count - 1; i++)
            {
                var mesh = new Mesh
                {
                    name = "Procedural Mesh " + i
                };

                var track = new GameObject($"Knot Segment_{i}");

                track.AddComponent<MeshFilter>();
                track.AddComponent<MeshRenderer>();
                track.AddComponent<MeshCollider>().sharedMesh = mesh;
                track.AddComponent<KnotSegment>();

                _meshes.Add(mesh);
                _tracks.Add(track.GetComponent<KnotSegment>());

                track.transform.parent = properties.transform;
                track.transform.position = properties.transform.position;
                track.transform.rotation = properties.transform.rotation;

                track.GetComponent<MeshFilter>().sharedMesh = mesh;
                track.GetComponent<MeshRenderer>().sharedMaterial = properties.Material;

                _trackConstructor ??= new TrackConstructor();

                _trackConstructor.ConstructBezierCurveVertices(properties, mesh, i);
                _trackConstructor.ConstructBezierCurveNormals(properties, mesh);
                _trackConstructor.ConstructBezierUVs(properties, mesh);
                _trackConstructor.ConstructTriangles(properties, mesh, properties.SplinePointsCount - 1);

                _trackConstructor.ClearMeshData();
            }

            //LoadSegmentData();
        }

        public void ClearTrack(TrackProperties properties)
        {
            switch (properties.Mode)
            {
                case TrackMode.Single:
                    _trackConstructor.ClearMeshData();
                    if (_mesh) _mesh.Clear();
                    break;
                case TrackMode.KnotBased:
                    Debug.Log("Not Implemented YET");
                    break;
                default:
                    Debug.Log("Not Implemented");
                    break;
            }
        }

        /// <summary>
        /// Deletes mesh and "track" game object
        /// Clear arrays data
        /// </summary>
        public void DeleteTrackCompletely(TrackProperties properties)
        {
            switch (properties.Mode)
            {
                case TrackMode.Single:
                    DeleteSingle();
                    DeleteMultiple();
                    break;
                case TrackMode.KnotBased:
                    DeleteSingle();
                    DeleteMultiple();
                    break;
                default:
                    Debug.Log("Not Implemented");
                    break;
            }
        }

        public void DeleteOnValidate(TrackProperties properties)
        {
            switch (properties.Mode)
            {
                case TrackMode.Single:
                    DeleteSingleFromValidate();
                    DeleteMultipleTypeFromValidate();
                    break;
                case TrackMode.KnotBased:
                    DeleteSingleFromValidate();
                    DeleteMultipleTypeFromValidate();
                    break;
                default:
                    Debug.Log("Not Implemented");
                    break;
            }
        }

        private void DeleteSingle()
        {
            DestroyImmediate(_mesh);
            DestroyImmediate(_track);

            _trackConstructor.ClearMeshData();
        }

        private void DeleteSingleFromValidate()
        {
            if (_mesh)
            {
                _mesh.Clear();
                EditorApplication.delayCall += () => { DestroyImmediate(_mesh); };
            }

            if (_track)
            {
                EditorApplication.delayCall += () => { DestroyImmediate(_track); };
            }

            _trackConstructor.ClearMeshData();
        }

        private void DeleteMultiple()
        {
            //SaveSegmentData();

            if (_meshes == null || _meshes.Count == 0) return;
            if (_tracks == null || _tracks.Count == 0) return;

            foreach (var mesh in _meshes)
            {
                DestroyImmediate(mesh);
            }

            _meshes.Clear();

            foreach (var knotSegment in _tracks)
            {
                DestroyImmediate(knotSegment.gameObject);
            }

            _tracks.Clear();

            _trackConstructor.ClearMeshData();
        }

        private void DeleteMultipleTypeFromValidate()
        {
            //SaveSegmentData();

            if (_meshes == null || _meshes.Count == 0) return;
            if (_tracks == null || _tracks.Count == 0) return;

            foreach (var mesh in _meshes)
            {
                EditorApplication.delayCall += () => { DestroyImmediate(mesh); };
            }

            foreach (var knotSegment in _tracks)
            {
                EditorApplication.delayCall += () => { DestroyImmediate(knotSegment.gameObject); };
            }

            _meshes.Clear();
            _tracks.Clear();

            _trackConstructor.ClearMeshData();
        }

        private void LoadSegmentData()
        {
            _segmentResolutionData = ApplicationPrefs.GetObject<SegmentResolutionData>(Constants.SEGMENT_DATA);

            if (_segmentResolutionData == null || _segmentResolutionData.Values.Count == 0) return;

            for (int i = 0; i < _segmentResolutionData.Values.Count; i++)
            {
                _tracks[i].Resolution = _segmentResolutionData.Values[i];
            }
        }

        private void SaveSegmentData()
        {
            if (_tracks == null) return;

            _segmentResolutionData.Values.Clear();
            //Debug.Log(_tracks.Count);

            for (int i = 0; i < _tracks.Count; i++)
            {
                if (_tracks is { Count: > 0 })
                {
                    _segmentResolutionData.Values.Add(_tracks[i].Resolution);
                }
            }

            //Debug.Log(_segmentResolutionData.Values.Count);

            ApplicationPrefs.SetObject(Constants.SEGMENT_DATA, _segmentResolutionData);
        }
    }
}