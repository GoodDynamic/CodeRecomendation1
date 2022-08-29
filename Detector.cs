using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]

public class Detector : MonoBehaviour
{
    [SerializeField] private UnityEvent _intruderEnter;
    [SerializeField] private UnityEvent _intruderExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ThiefMotion thiefMotion))
        {
            if (other.transform.position.z > transform.position.z)
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
