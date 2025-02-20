using UnityEngine;

public sealed class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _smoothFactor;

    private Vector3 _animatorDirection;
    private Vector3 _animationDirectionSmooth;

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        _animatorDirection = _playerController.Root.InverseTransformDirection(
            _playerController.MoveDirection
        );

        _animationDirectionSmooth = Vector3.Lerp(
            _animationDirectionSmooth,
            _animatorDirection,
            _smoothFactor * 50 * Time.deltaTime
        );

        _animator.SetFloat("Horizontal", _animationDirectionSmooth.x);
        _animator.SetFloat("Vertical", _animationDirectionSmooth.z);
    }
}
