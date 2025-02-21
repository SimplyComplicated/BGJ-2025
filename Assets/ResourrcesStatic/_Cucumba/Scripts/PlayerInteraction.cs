using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInteraction : MonoBehaviour
{
    public event Action<IInteractableObject> OnInteractionEnter;
    public event Action<IInteractableObject> OnInteractionExit;
    public event Action OnInteractionStateChanged;

    private const int kBufferSize = 16;

    [SerializeField]
    private Transform _origin;

    [SerializeField]
    private float _radiusOfHighlight = 10;

    [SerializeField]
    private float _radiusOfInteract = 1;

    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    private KeyCode _keyCode;

    private Camera _camera;
    private InteractionManager _interactionManager;

    private readonly Collider[] _colliders = new Collider[kBufferSize];

    private readonly List<IInteractableObject> _currentInteractables = new(kBufferSize);
    private IInteractableObject _currentInteraction;

    public IInteractableObject CurrentInteraction => _currentInteraction;

    private void Awake()
    {
        _camera = Camera.main;
        _interactionManager = FindFirstObjectByType<InteractionManager>();

        if (_origin == null)
        {
            _origin = transform;
        }
    }

    private void Update()
    {
        HandleHighlight();
        HandleInteraction();
        HandleKeyboard();
    }

    private void OnDrawGizmos()
    {
        if (_origin == null)
        {
            return;
        }

        var pos = _origin.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos, _radiusOfHighlight);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pos, _radiusOfInteract);
    }

    private void HandleHighlight()
    {
        _currentInteractables.Clear();

        var numColliders = Physics.OverlapSphereNonAlloc(
            _origin.position,
            _radiusOfHighlight,
            _colliders
        );

        for (int i = 0; i < numColliders; i++)
        {
            var item = _colliders[i].gameObject;

            if (item.TryGetComponent<IInteractableObject>(out var interactable))
            {
                _currentInteractables.Add(interactable);
            }
        }

        _interactionManager.Highlight(_currentInteractables);
    }

    private void HandleInteraction()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, 777, _layerMask))
        {
            var flag =
                hit.collider.gameObject.TryGetComponent<IInteractableObject>(out var interaction)
                && interaction.IsActive()
                && Vector3.Distance(_origin.position, interaction.WorldPosition)
                    < _radiusOfInteract;

            if (flag)
            {
                SetCurrentInteraction(interaction);
            }
            else
            {
                SetCurrentInteraction(null);
            }
        }
        else
        {
            SetCurrentInteraction(null);
        }
    }

    private void HandleKeyboard()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            if (_currentInteraction != null)
            {
                _currentInteraction.Use();
            }
        }
    }

    private void SetCurrentInteraction(IInteractableObject interaction)
    {
        if (_currentInteraction == null && interaction != null)
        {
            _currentInteraction = interaction;
            OnEnterInternal(_currentInteraction);
            OnStateChangedInternal();
        }
        else if (_currentInteraction != null && interaction == null)
        {
            OnExitInternal(_currentInteraction);
            _currentInteraction = null;
            OnStateChangedInternal();
        }
        else if (
            _currentInteraction != null
            && interaction != null
            && _currentInteraction != interaction
        )
        {
            OnExitInternal(_currentInteraction);
            _currentInteraction = interaction;
            OnEnterInternal(_currentInteraction);
            OnStateChangedInternal();
        }
    }

    private void OnEnterInternal(IInteractableObject item)
    {
        item.OnEnter();
        OnInteractionEnter?.Invoke(item);
    }

    private void OnExitInternal(IInteractableObject item)
    {
        item.OnExit();
        OnInteractionExit?.Invoke(item);
    }

    private void OnStateChangedInternal()
    {
        OnInteractionStateChanged?.Invoke();
    }
}
