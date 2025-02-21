using System;

public interface IHackableObject : IInteractableObject
{
    event Action OnStateChanged;

    float Progress01 { get; }

    bool IsBusy { get; }

    void OnHackingSuccess();

    void OnHackingFailed();
}
