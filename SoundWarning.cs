using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundWarning : MonoBehaviour
{
    private readonly float _minLoud = 0;
    private readonly float _maxLoud = 1;
    private float _loud = 0;
    private float _loudStep = 0.01f;

    private Coroutine _onLoudChange;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnIntruderEnter()
    {
        if (_audioSource.isPlaying == false)
            _audioSource.Play();

        if (_onLoudChange != null)
            StopCoroutine(_onLoudChange);

        _audioSource.mute = false;
        _onLoudChange = StartCoroutine(ChangeLoudness(_maxLoud));
    }

    public void OnIntruderExit()
    {
        StopCoroutine(_onLoudChange);
        _onLoudChange = StartCoroutine(ChangeLoudness(_minLoud));
    }

    private IEnumerator ChangeLoudness(float targetLoud)
    {
        while (_loud != targetLoud)
        {
            _loud = Mathf.MoveTowards(_loud, targetLoud, _loudStep);
            _audioSource.volume = _loud;
            yield return null;
        }

        if (targetLoud == _minLoud)
        {
            _audioSource.mute = true;
        }
    }
}
