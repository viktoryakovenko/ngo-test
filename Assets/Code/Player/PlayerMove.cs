using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Services.Inputs;
using Code.StaticData;
using Unity.Netcode;
using UnityEngine;

namespace Code.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMove : NetworkBehaviour
    {
        [SerializeField] private CharacterController _characterController;

        private float _movementSpeed;
        private IInputService _inputService;

        public override void OnNetworkSpawn()
        {
            _inputService = new InputService();
            _movementSpeed = Resources.Load<PlayerConfig>(AssetPath.HeroConfigPath).MovementSpeed;
        }

        private void Update()
        {
            if (!IsOwner) return;

            var movementVector = MovementVector();

            if (IsServer && IsLocalPlayer)
                Move(movementVector);
            else if (IsLocalPlayer)
            {
                MoveServerRpc(movementVector);
            }
        }

        private void Move(Vector3 movementVector) =>
            _characterController.Move(_movementSpeed * Time.deltaTime * movementVector);

        [ServerRpc]
        private void MoveServerRpc(Vector3 movementInput) =>
            Move(movementInput);


        private Vector3 MovementVector()
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                movementVector = Camera.main.transform.TransformDirection(_inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;
            return movementVector;
        }
    }
}