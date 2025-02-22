using System.Collections.Generic;
using Game.Utilities;
using UnityEngine;

public sealed class PlayerFootstep : MonoBehaviour
{
    [SerializeField]
    private PlayerController _controller;

    [SerializeField]
    private float _timeInterval;

    [SerializeField]
    private string[] _sfxNames;

    private AudioManager _audioManager;

    private float _timer;

    private List<string> _shuffledSfxNames;

    private void Awake()
    {
        _audioManager = FindAnyObjectByType<AudioManager>();

        _shuffledSfxNames = new List<string>(_sfxNames);

        _shuffledSfxNames.Shuffle(new System.Random(0));
    }

    private void Update()
    {
        HandleTick();
    }

    private void HandleTick()
    {
        if (_controller.IsMoving)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _timer = 0f;
        }

        if (_timer >= _timeInterval)
        {
            _timer = 0f;

            PlayRandomSound();
        }
    }

    private void PlayRandomSound()
    {
        if (_audioManager == null)
        {
            return;
        }

        if (_sfxNames.Length == 0)
        {
            return;
        }

        var idx = Random.Range(0, _shuffledSfxNames.Count);
        var name = _shuffledSfxNames[idx];
        _audioManager.PlaySFXWithRandomPitch(name);
    }
}
