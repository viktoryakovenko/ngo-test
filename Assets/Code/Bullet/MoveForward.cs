using Unity.Netcode;
using UnityEngine;

namespace Code.Bullet
{
    public class MoveForward : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _lifetime;

        private Vector3 _direction;

        public void Initialize(Vector3 direction)
        {
            _direction = direction.normalized;
            Destroy(gameObject, _lifetime);
        }

        private void Update()
        {
            if (IsServer)
                transform.position += _speed * Time.deltaTime * _direction;
        }
    }
}
