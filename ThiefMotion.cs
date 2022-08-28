using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Animator))]

public class ThiefMotion : MonoBehaviour
{
    [SerializeField] private int _reachedPoints = 0;
    private readonly int _stepsToDistrict = 20;

    [SerializeField] private bool _isWorried;
    private float _accessTime = 4;
    private bool _isAccessing = false;

    [SerializeField] private GameObject _allPath;
    [SerializeField] private Transform[] _path;
    private Transform[] _backPath;
    private Animator _animator;

    void Start()
    {
        _animator=GetComponent<Animator>();
        _animator.SetFloat("speed", 1f);

        _path =_allPath.GetComponentsInChildren<Transform>().
               OrderBy(transform => transform.gameObject.name).ToArray();
        _path = _path.Except(new Transform[] { _allPath.GetComponent<Transform>() }).ToArray();
        _backPath = _path.Reverse().ToArray();
    }

    void Update()
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
            _animator.SetFloat("speed", 1f);

            if ((transform.position - path[_reachedPoints + 1].position).magnitude < 0.1)
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
                transform.position = Vector3.MoveTowards(transform.position, path[_reachedPoints + 1].position, 1f / (float)_stepsToDistrict);
            }
        }
        else
        {
            AccessSituation();
        }
    }

    private Quaternion GetDirectRotation(Vector3 startPosition,Vector3 nextPosition)
    {
        Vector3 lookDirection = (nextPosition - startPosition);
        lookDirection.y = 0;
        return Quaternion.LookRotation(lookDirection, Vector3.up);
    }

    private void AccessSituation()
    {
        GetComponent<Animator>().SetFloat("speed", 0f);
        _isAccessing = true;
        _isWorried = !_isWorried;

        Invoke(nameof(StartNewMotion), _accessTime);
    }

    private void StartNewMotion()
    {
        _animator.SetFloat("speed", 1f);
        _reachedPoints = 0;
        _isAccessing = false;
    }
}
