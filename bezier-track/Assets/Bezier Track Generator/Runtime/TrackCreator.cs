using System;
using System.Collections.Generic;
using System.Linq;
using ptl.bezier.Enums;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace ptl.bezier
{
    public class TrackCreator : MonoBehaviour
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private GameObject _track;
        [SerializeField] private List<Mesh> _meshes;
        [SerializeField] private List<GameObject> _tracks;
        [SerializeField] private TrackConstructor _trackConstructor;

        public void CreateTrack(TrackProperties properties)
        {
            switch (properties.TrackType)
            {
                case TrackType.Single:
                    CreateSingleTrack(properties);
                    break;
                case TrackType.Multiple:
                    CreateMultipleTrack(properties);
                    break;
                case TrackType.None:
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

                _trackConstructor.ConstructTriangles(properties, mesh);
                _trackConstructor.ClearMeshData();
            }
        }

        public void ClearTrack(TrackProperties properties)
        {
            switch (properties.TrackType)
            {
                case TrackType.Single:
                    _trackConstructor.ClearMeshData();
                    if (_mesh) _mesh.Clear();
                    break;
                case TrackType.Multiple:
                    break;
                case TrackType.None:
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
            switch (properties.TrackType)
            {
                case TrackType.Single:
                    DeleteMultiple();
                    DeleteSingle();
                    break;
                case TrackType.Multiple:
                    DeleteSingle();
                    DeleteMultiple();
                    break;
                case TrackType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void DeleteOnValidate(TrackProperties properties)
        {
            switch (properties.TrackType)
            {
                case TrackType.Single:
                    DeleteMultipleTypeFromValidate();
                    DeleteSingleFromValidate();
                    break;
                case TrackType.Multiple:
                    DeleteSingleFromValidate();
                    DeleteMultipleTypeFromValidate();

                    break;
                case TrackType.None:
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
            if (_mesh) _mesh.Clear();
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