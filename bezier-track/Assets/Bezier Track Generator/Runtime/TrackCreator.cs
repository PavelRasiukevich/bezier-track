using UnityEngine;
using UnityEngine.Serialization;

namespace ptl.bezier
{
    public class TrackCreator : MonoBehaviour
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private GameObject _track;
        [SerializeField] private TrackConstructor _trackConstructor;

        public void CreateTrack(TrackProperties properties)
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

        public void ClearTrack()
        {
            _trackConstructor.ClearMeshData();
            if (_mesh) _mesh.Clear();
        }

        /// <summary>
        /// Deletes mesh and "track" game object
        /// Clear arrays data
        /// </summary>
        public void DeleteTrackCompletely()
        {
            DestroyImmediate(_mesh);
            DestroyImmediate(_track);
            _trackConstructor = null;
        }
    }
}