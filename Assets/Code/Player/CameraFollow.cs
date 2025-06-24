using Unity.Netcode;
using UnityEngine;

namespace Code.Player
{
    public class CameraFollow : NetworkBehaviour
    {
        [SerializeField] private Vector3 _offset = new Vector3(0f, 5f, -8f);
        private Camera _camera;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            CameraSetup();
        }

        private void LateUpdate()
        {
            if (!IsOwner || _camera == null) return;

            UpdateCameraPosition();
        }

        private void CameraSetup()
        {
            _camera = Camera.main;

            if (_camera != null)
            {
                _camera.transform.SetParent(transform);
                _camera.transform.localPosition = _offset;
                _camera.transform.LookAt(transform);
            }
        }

        private void UpdateCameraPosition()
        {
            _camera.transform.position = transform.position + _offset;
            _camera.transform.LookAt(transform);
        }
    }
}
