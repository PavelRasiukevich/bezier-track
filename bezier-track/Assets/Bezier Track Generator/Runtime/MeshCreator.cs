using UnityEngine;

namespace SplineRoad.BezierTrack
{
    public class MeshCreator : MonoBehaviour
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private GameObject _track;
        [SerializeField] private TrackConstructor _trackConstructor;

        public void CreateMesh(TrackProperties properties)
        {
            //if (!properties.MeshDataContainer) return;

            if (_track == null)
            {
                _track = new GameObject("Track");
                _track.AddComponent<MeshFilter>();
                _track.AddComponent<MeshRenderer>();
                _track.transform.parent = properties.transform;
            }

            if (_mesh == null)
            {
                _mesh = new Mesh
                {
                    name = "Procedural Track"
                };
            }

            _trackConstructor ??= new TrackConstructor();

            _track.GetComponent<MeshFilter>().sharedMesh = _mesh;
            _track.GetComponent<MeshRenderer>().sharedMaterial = properties.Material;
            
            _trackConstructor.ConstructTrack(properties, _mesh);
        }

        public void ClearMesh()
        {
            if (_mesh) _mesh.Clear();
        }

        /// <summary>
        /// Deletes mesh and "track" game object
        /// Clear arrays data
        /// </summary>
        public void DeleteCompletely()
        {
            DestroyImmediate(_mesh);
            DestroyImmediate(_track);
            _trackConstructor = null;
        }
    }
}