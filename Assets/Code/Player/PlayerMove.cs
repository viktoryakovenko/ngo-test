using Unity.Netcode;
using UnityEngine;

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

        Move();
    }

    private void Move()
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

        _characterController.Move(_movementSpeed * Time.deltaTime * movementVector);
    }

    private static Vector2 Axis() =>
        new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
}