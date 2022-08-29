using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class SoundWarning : MonoBehaviour
{
    private readonly float _minLoud = 0;
    private readonly float _maxLoud = 1;
    private float _loud = 0;
    private float _loudStep = 0.01f;

    private Coroutine _onEnterCoroutine;
    private Coroutine _onExitCoroutine;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnIntruderEnter()
    {
        if (_audioSource.isPlaying == false)
            _audioSource.Play();

        if (_onExitCoroutine != null)
            StopCoroutine(_onExitCoroutine);

        _audioSource.mute = false;
        _onEnterCoroutine = StartCoroutine(ChangeLoudness(_maxLoud));
    }

    public void OnIntruderExit()
    {
        StopCoroutine(_onEnterCoroutine);

        _onExitCoroutine = StartCoroutine(ChangeLoudness(_minLoud));
    }

    private IEnumerator ChangeLoudness(float targetLoud)
    {
        while (Mathf.Abs(_loud - targetLoud) > _loudStep)
        {
            _loud = Mathf.Lerp(_loud, targetLoud, _loudStep);
            _audioSource.volume = _loud;

            yield return null;
        }

        _loud = targetLoud;

        if (targetLoud == _minLoud)
            _audioSource.mute = true;
    }
}
