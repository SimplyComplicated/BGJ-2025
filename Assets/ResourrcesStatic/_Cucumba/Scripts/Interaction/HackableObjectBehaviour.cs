using System;
using UnityEngine;
using UnityEngine.Events;

public class HackableObjectBehaviour : InteractableObjectBehaviour, IHackableObject
{
    public event Action OnStateChanged;

    public UnityEvent onHackingSuccess;

    public UnityEvent onHackingFailed;

    [SerializeField]
    private float _hackingTime = 2;

    private bool _isBusy = false;
    private float _progress01 = 0;

    public float Progress01 => _progress01;
    public bool IsBusy => _isBusy;

    public override void Use()
    {
        if (!_isBusy)
        {
            _isBusy = true;

            SyncState();
        }
    }

    public virtual void OnHackingSuccess()
    {
        onHackingSuccess.Invoke();
    }

    public virtual void OnHackingFailed()
    {
        onHackingFailed.Invoke();
    }

    public override bool IsActive()
    {
        return _progress01 < 1f;
    }

    public override void OnExit()
    {
        if (_isBusy && IsActive())
        {
            _isBusy = false;
            _progress01 = 0f;

            SyncState();
        }
    }

    private void Update()
    {
        if (_isBusy)
        {
            _progress01 += 1f / _hackingTime * Time.deltaTime;

            if (_progress01 >= 1f)
            {
                _isBusy = false;

                SyncState();

                OnHackingSuccess();
            }
        }
    }

    private void SyncState()
    {
        OnStateChanged?.Invoke();
    }
}
