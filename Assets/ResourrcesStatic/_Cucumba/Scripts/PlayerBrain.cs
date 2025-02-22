using System;
using UnityEngine;

public sealed class PlayerBrain : MonoBehaviour
{
    public event Action OnDeath;

    [SerializeField]
    private PlayerAnimation _animation;

    [SerializeField]
    private PlayerController _controller;

    public void Rip()
    {
        _animation.Death(true);

        _controller.enabled = false;

        OnDeath?.Invoke();
    }
}
