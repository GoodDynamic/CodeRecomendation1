using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]

public class Detector : MonoBehaviour
{
    [SerializeField] private UnityEvent _intruderEnter;
    [SerializeField] private UnityEvent _intruderExit;

    [SerializeField] private bool _hasDetection;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ThiefMotion thiefMotion))
        {
            _hasDetection = !_hasDetection;

            if (_hasDetection)
            {
                _intruderEnter.Invoke();
            }
            else
            {
                _intruderExit.Invoke();
            }
        }
    }
}
