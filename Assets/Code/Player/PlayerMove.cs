using Unity.Netcode;
using UnityEngine;

namespace Code.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMove : NetworkBehaviour
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _movementSpeed;

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

        private Vector2 Axis() =>
            new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));

        private Vector3 MovementVector()
        {
            Vector3 movementVector = Vector3.zero;

            if (Axis().sqrMagnitude > Constants.Epsilon)
            {
                movementVector = Camera.main.transform.TransformDirection(Axis());
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;
            return movementVector;
        }
    }
}