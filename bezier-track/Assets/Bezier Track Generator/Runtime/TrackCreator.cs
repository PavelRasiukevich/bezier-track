using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ptl.bezier
{
    public class TrackCreator : MonoBehaviour
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private GameObject _track;
        [SerializeField] private List<Mesh> _meshes;
        [SerializeField] private List<GameObject> _tracks;
        [SerializeField] private TrackConstructor _trackConstructor;

        public TrackConstructor TrackConstructor => _trackConstructor;

        public void CreateTrack(TrackProperties properties)
        {
            switch (properties.Mode)
            {
                case TrackMode.Single:
                    CreateSingleTrack(properties);
                    break;
                case TrackMode.Multiple:
                    CreateMultipleTrack(properties);
                    break;
                case TrackMode.KnotBased:
                    CreateTrackBasedOnKnots(properties);
                    break;
                case TrackMode.None:
                default:
                    throw new NotImplementedException();
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
            var segments = properties.SplinePointsCount - 1;

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

                for (int j = 0; j < properties.SegmentCount; j++)
                {
                    _trackConstructor.ConstructVertices(properties, mesh, j + i);
                    _trackConstructor.ConstructNormals(properties, mesh, j + i);
                    _trackConstructor.ConstructUVs(properties, mesh, j + i);
                }

                _trackConstructor.ConstructTriangles(properties, mesh, 1);
                _trackConstructor.ClearMeshData();
            }
        }

        private void CreateTrackBasedOnKnots(TrackProperties properties)
        {
            for (int i = 0; i < properties.SplineContainer.Spline.Count - 1; i++)
            {
                var mesh = new Mesh
                {
                    name = "Procedural Mesh Based On Knot " + i
                };

                var track = new GameObject($"Knot Track Segment_{i}");

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

                _trackConstructor.ConstructBezierCurveVertices(properties, mesh, i);
                _trackConstructor.ConstructBezierCurveNormals(properties, mesh);
                _trackConstructor.ConstructBezierUVs(properties, mesh);
                _trackConstructor.ConstructTriangles(properties, mesh, properties.SplinePointsCount - 1);
                
                _trackConstructor.ClearMeshData();
            }
        }

        public void ClearTrack(TrackProperties properties)
        {
            switch (properties.Mode)
            {
                case TrackMode.Single:
                    _trackConstructor.ClearMeshData();
                    if (_mesh) _mesh.Clear();
                    break;
                case TrackMode.Multiple:
                    Debug.Log("Not Implemented YET");
                    break;
                case TrackMode.KnotBased:
                    Debug.Log("Not Implemented YET");
                    break;
                case TrackMode.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
                    DeleteMultiple();
                    DeleteSingle();
                    break;
                case TrackMode.Multiple:
                    DeleteSingle();
                    DeleteMultiple();
                    break;
                case TrackMode.KnotBased:
                    DeleteSingle();
                    DeleteMultiple();
                    break;
                case TrackMode.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void DeleteOnValidate(TrackProperties properties)
        {
            switch (properties.Mode)
            {
                case TrackMode.Single:
                    DeleteMultipleTypeFromValidate();
                    DeleteSingleFromValidate();
                    break;
                case TrackMode.Multiple:
                    DeleteSingleFromValidate();
                    DeleteMultipleTypeFromValidate();
                    break;
                case TrackMode.KnotBased:
                    DeleteSingleFromValidate();
                    DeleteMultipleTypeFromValidate();
                    break;
                case TrackMode.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DeleteSingle()
        {
            DestroyImmediate(_mesh);
            DestroyImmediate(_track);
            _trackConstructor = null;
        }

        private void DeleteSingleFromValidate()
        {
            _trackConstructor.ClearMeshData();
            if (_mesh)
            {
                _mesh.Clear();
                EditorApplication.delayCall += () => { DestroyImmediate(_mesh); };
            }

            if (_track)
            {
                EditorApplication.delayCall += () => { DestroyImmediate(_track); };
            }
        }

        private void DeleteMultiple()
        {
            foreach (var mesh in _meshes)
            {
                DestroyImmediate(mesh);
            }

            _meshes.Clear();

            foreach (var track in _tracks)
            {
                DestroyImmediate(track);
            }

            _tracks.Clear();

            _trackConstructor = null;
        }

        private void DeleteMultipleTypeFromValidate()
        {
            foreach (var mesh in _meshes)
            {
                EditorApplication.delayCall += () => { DestroyImmediate(mesh); };
            }

            foreach (var go in _tracks)
            {
                EditorApplication.delayCall += () => { DestroyImmediate(go); };
            }

            _meshes.Clear();
            _tracks.Clear();
        }
    }
}