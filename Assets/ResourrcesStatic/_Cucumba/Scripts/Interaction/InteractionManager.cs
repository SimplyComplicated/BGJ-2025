using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class InteractionManager : MonoBehaviour
{
    private IInteractableObject[] _interactableObjects;

    private HashSet<IInteractableObject> _active = new();

    private void Awake()
    {
        _interactableObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Select(x => x.GetComponent<IInteractableObject>())
            .Where(x => x != null)
            .ToArray();

        foreach (var item in _interactableObjects)
        {
            item.Highlight(false);
        }
    }

    public void Highlight(IList<IInteractableObject> items)
    {
        foreach (var item in _active)
        {
            if (!items.Contains(item))
            {
                OnExit(item);
            }
        }

        foreach (var item in items)
        {
            if (!_active.Contains(item))
            {
                OnEnter(item);
            }
        }

        _active.Clear();
        foreach (var item in items)
        {
            _active.Add(item);
        }
    }

    private void OnEnter(IInteractableObject item)
    {
        item.Highlight(true);
    }

    private void OnExit(IInteractableObject item)
    {
        item.Highlight(false);
    }
}
