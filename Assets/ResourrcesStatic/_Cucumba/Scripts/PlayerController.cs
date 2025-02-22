using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    private float _moveSpeed = 1;

    [SerializeField]
    private float _smoothFactor;

    [SerializeField]
    private Transform _cameraOrigin;

    [SerializeField]
    private float _cameraMaxOffset;

    private Camera _camera;
    private Transform _cameraTransform;

    private Vector2 _input;
    private Vector3 _moveDirection;
    private Vector3 _mouseWorldPosition;

    private Transform _root;

    public Transform Root => _root;

    public Vector3 MoveDirection => _moveDirection;

    public bool IsMoving => _moveDirection.magnitude > 0;

    private void Awake()
    {
        _camera = Camera.main;
        _cameraTransform = _camera.transform;
        _root = _characterController.transform;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleCamera();
    }

    private void HandleMovement()
    {
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

        var forward = _cameraTransform.forward * _input.y;
        forward.y = 0;
        forward.Normalize();

        var right = _cameraTransform.right * _input.x;

        _moveDirection = forward + right;
        _moveDirection.Normalize();

        var motion = _moveSpeed * Time.deltaTime * _moveDirection;

        _characterController.Move(motion);
    }

    private void HandleRotation()
    {
        var plane = new Plane(Vector3.up, Vector3.zero);
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out var distance))
        {
            _mouseWorldPosition = ray.GetPoint(distance);

            var dir = _mouseWorldPosition - _root.position;
            dir.y = 0;

            var targetRotation = Quaternion.LookRotation(dir);

            _root.rotation = Quaternion.Lerp(
                _root.rotation,
                targetRotation,
                _smoothFactor * 50 * Time.deltaTime
            );
        }
    }

    private void HandleCamera()
    {
        Vector2 mouseViewportPos = _camera.ScreenToViewportPoint(Input.mousePosition);
        mouseViewportPos.x = Mathf.Clamp01(mouseViewportPos.x);
        mouseViewportPos.y = Mathf.Clamp01(mouseViewportPos.y);

        var mouseOffset = mouseViewportPos;
        mouseOffset *= 2f;
        mouseOffset -= Vector2.one;

        var forward = _cameraTransform.forward * mouseOffset.y;
        forward.y = 0;

        var right = _cameraTransform.right * mouseOffset.x;

        var axis = forward + right;

        _cameraOrigin.position = _root.position + axis * _cameraMaxOffset;
    }
}
