using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class SoundWarning : MonoBehaviour
{
    private readonly float _minLoud = 0;
    private readonly float _maxLoud = 1;
    private float _loud=0;
    private int _maxFramesCount = 200;

    private Coroutine _onEnterCoroutine;
    private Coroutine _onExitCoroutine;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource=GetComponent<AudioSource>();
    }

    public void OnIntruderEnter()
    {
        _audioSource.Play();
        
        if (_onExitCoroutine != null)
            StopCoroutine(_onExitCoroutine);

        _onEnterCoroutine = StartCoroutine(ChangeLoudness(_maxLoud));
    }

    public void OnIntruderExit()
    {
        StopCoroutine(_onEnterCoroutine);

        _onExitCoroutine = StartCoroutine(ChangeLoudness(_minLoud));
    }

    private IEnumerator ChangeLoudness(float targetLoud)
    {
        while (Mathf.Abs(_loud - targetLoud) > 1f / _maxFramesCount)
        {
            _loud = Mathf.Lerp(_loud, targetLoud, 1f / _maxFramesCount);
            _audioSource.volume = _loud;

            yield return null;
        }
    }
}
