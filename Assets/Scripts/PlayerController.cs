using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Stamina))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0)] private float _walkSpeed;
    [SerializeField, Min(0)] private float _runSpeed;
    [SerializeField, Min(0)] private float _jumpForce;
    [SerializeField, Min(0)] private float _gravityScale;

    [Header("Stamina usage")]
    [SerializeField, Min(0)] private float _runStaminaUsage;
    [SerializeField, Min(0)] private float _jumpStaminaUsage;

    [Header("View")]
    [SerializeField] private CameraShaker _shaker;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField, Min(0)] private float _mouseSensitivity;

    private float _movementSpeed;
    private Vector3 _movementDirection;
    private Vector3 _movementVelocity;
    private CharacterController _characterController;
    private Stamina _stamina;

    private float _cameraPitch;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _stamina = GetComponent<Stamina>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
 
    private void Update()
    {
        UpdateCharacterMovement();
        UpdateCameraRotation();
    }

    private void UpdateCharacterMovement()
    {
        _movementDirection = new Vector3
        (
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        ).normalized;
        _movementDirection = transform
            .TransformDirection(_movementDirection);

        if (_characterController.isGrounded)
        {
            _movementVelocity.y = -1;
            if (Input.GetKeyDown(KeyCode.Space) && _stamina.UseStamina(_jumpStaminaUsage))
                _movementVelocity.y = _jumpForce;
        }
        else
            _movementVelocity.y -= _gravityScale * Time.deltaTime;

        if (_movementDirection != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift) && _stamina.UseStamina(_runStaminaUsage * Time.deltaTime))
            {
                _movementSpeed = _runSpeed;
                _shaker.SetActiveShaker();
            }
            else
            {
                _movementSpeed = _walkSpeed;
                _shaker.SetActiveShaker(false);
            }
        }
        else
            _shaker.SetActiveShaker(false);

        _characterController.Move((_movementDirection * _movementSpeed
                + _movementVelocity) * Time.deltaTime); 
    }

    private void UpdateCameraRotation()
    {
        Vector2 mousePosition = new Vector2
        (
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        _cameraPitch -= mousePosition.y * _mouseSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90, 90);
        _cameraTransform.localEulerAngles = Vector3.right * _cameraPitch;

        transform.Rotate(Vector3.up * mousePosition.x * _mouseSensitivity);
    }
}
