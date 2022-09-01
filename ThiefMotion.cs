using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Animator))]

public class ThiefMotion : MonoBehaviour
{
    [SerializeField] private GameObject _pathPoints;
    private Transform[] _path;
    private Transform[] _backPath;

    private int _reachedPoints;
    private readonly float _stepSize = 0.05f;

    private bool _isWorried;
    private readonly float _accessTime = 4;
    private bool _isAccessing;

    private Animator _animator;
    private float _walkSpeed = 1f;
    private const string _thiefsSpeed = "thiefsSpeed";

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetFloat(_thiefsSpeed, _walkSpeed);
        _path = _pathPoints.GetComponentsInChildren<Transform>().
               OrderBy(transform => transform.gameObject.name).ToArray();
        _path = _path.Except(new Transform[] { _pathPoints.GetComponent<Transform>() }).ToArray();
        _backPath = _path.Reverse().ToArray();
    }

    private void Update()
    {
        if (_isAccessing == false)
            if (_isWorried)
                FollowPath(_backPath);
            else
                FollowPath(_path);
    }

    private void FollowPath(Transform[] path)
    {
        if (_reachedPoints + 1 < path.Length)
        {
            _animator.SetFloat(_thiefsSpeed, _walkSpeed);

            if ((transform.position - path[_reachedPoints + 1].position).magnitude < _stepSize)
            {
                _reachedPoints++;

                if (_reachedPoints + 1 < path.Length)
                {
                    transform.rotation = GetDirectRotation(transform.position, path[_reachedPoints + 1].position);
                }
            }
            else
            {
                transform.rotation = GetDirectRotation(transform.position, path[_reachedPoints + 1].position);
                transform.position = Vector3.MoveTowards(transform.position, path[_reachedPoints + 1].position, _stepSize);
            }
        }
        else
        {
            AccessSituation();
        }
    }

    private Quaternion GetDirectRotation(Vector3 startPosition, Vector3 nextPosition)
    {
        Vector3 lookDirection = (nextPosition - startPosition);
        lookDirection.y = 0;
        return Quaternion.LookRotation(lookDirection, Vector3.up);
    }

    private void AccessSituation()
    {
        _animator.SetFloat(_thiefsSpeed, 0);
        _isAccessing = true;
        _isWorried = !_isWorried;

        Invoke(nameof(StartNewMotion), _accessTime);
    }

    private void StartNewMotion()
    {
        _animator.SetFloat(_thiefsSpeed, _walkSpeed);
        _reachedPoints = 0;
        _isAccessing = false;
    }
}
