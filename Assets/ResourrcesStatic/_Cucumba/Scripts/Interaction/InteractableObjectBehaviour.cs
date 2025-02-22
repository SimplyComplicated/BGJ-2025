using UnityEngine;
using UnityEngine.Events;

public class InteractableObjectBehaviour : MonoBehaviour, IInteractableObject
{
    [SerializeField]
    private GameObject _highlight;

    [SerializeField]
    private Transform _origin;

    public UnityEvent onUse;

    private void Awake()
    {
        if (_origin == null)
        {
            _origin = transform;
        }
    }

    public Vector3 WorldPosition => _origin.position;

    public virtual bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public virtual void Use()
    {
        onUse.Invoke();
    }

    public virtual void Highlight(bool active)
    {
        if (_highlight != null)
        {
            _highlight.SetActive(active);
        }
    }

    public virtual void OnEnter()
    {
        //
    }

    public virtual void OnExit()
    {
        //
    }
}
