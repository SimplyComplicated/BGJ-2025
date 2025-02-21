using UnityEngine;

public interface IInteractableObject
{
    Vector3 WorldPosition { get; }

    bool IsActive();

    void Use();

    void Highlight(bool active);

    void OnEnter();

    void OnExit();
}
