using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityMotor))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform[] _route = new Transform[] { };
    [SerializeField] float _timeBetweenPoints = 1f;

    float _stopTimer = 0f;

    int _routePoint = 0;
    Transform _target = null;

    EntityMotor _motor;
    bool _canMove => _stopTimer <= 0f;

    private void Awake()
    {
        _motor = GetComponent<EntityMotor>();

        FollowRoute(_routePoint);
    }

    private void Update()
    {
        _stopTimer -= Mathf.Min(_stopTimer, Time.deltaTime);

        if (_canMove)
        {
            if (_motor.AtDestination)
            {
                if (_routePoint + 1 >= _route.Length)
                    _routePoint = 0;
                else
                    _routePoint++;
            }

            FollowRoute(_routePoint);
        }
    }

    void FollowRoute(int point)
    {
        if (_route.Length != 0 && point < _route.Length)
            SetTarget(_route[_routePoint]);
    }

    void SetTarget(Transform target)
    {
        _target = target;
        _motor.MoveTo(_target.position);
    }
}
