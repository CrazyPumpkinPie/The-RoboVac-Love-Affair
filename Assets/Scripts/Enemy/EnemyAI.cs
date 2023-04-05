using UnityEngine;

[RequireComponent(typeof(EntityMotor))]
public class EnemyAI : MonoBehaviour
{
    [Header("Route")]
    [SerializeField] Transform[] _route = new Transform[] { };
    [SerializeField] Transform _routeOrigin;
    [SerializeField] [Tooltip("The distance to route origin at which enemy stops following player")] float _maxDistanceToRoute = 5f;
    //[SerializeField] float _timeBetweenPoints = 1f;
    [Header("General")]
    public Transform Eyes;
    public float SightRange = 5f;
    public float ViewAngle = 180f;
    [Header("Gizmos")]
    [SerializeField] float _pointMarkerSize = .3f;
    [SerializeField] Color _pointMarkerColor = Color.cyan;
    [SerializeField] Color _routeLineColor = Color.green;

    [HideInInspector] public float CosViewAngle { get; private set; } = 0f;

    bool _atRoute = true;

    float _stopTimer = 0f;

    int _routePoint = 0;
    Transform _target = null;

    EntityMotor _motor;
    bool _canMove => _stopTimer <= 0f;

    private void Awake()
    {
        _motor = GetComponent<EntityMotor>();
        CosViewAngle = Mathf.Cos(ViewAngle * Mathf.Deg2Rad);
    }

    private void Start()
    {
        FollowRoute(_routePoint);        
    }

    private void Update()
    {
        _stopTimer -= Mathf.Min(_stopTimer, Time.deltaTime);

        if (Vector3.Distance(transform.position, _routeOrigin.position) > _maxDistanceToRoute)
            _atRoute = true;

        if (_canMove && !_atRoute && _target != null)
            _motor.MoveTo(_target.position);
        else if (_canMove && _atRoute)
        {
            if (_motor.AtDestination ||_target == null)
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
            SetTarget(_route[point]);
    }

    void SetTarget(Transform target)
    {
        _target = target;
        _motor.MoveTo(_target.position);
    }

    private void OnDrawGizmosSelected()
    {
        if (_route == null || _route.Length <= 0)
            return;

        for (int i = 0; i < _route.Length; i++)
        {
            Gizmos.color = _pointMarkerColor;
            Gizmos.DrawWireSphere(_route[i].position, _pointMarkerSize);

            if (_route.Length > i + 1)
            {
                Gizmos.color = _routeLineColor;
                Gizmos.DrawLine(_route[i].position, _route[i + 1].position);
            }
        }
    }

    public void ReportTarget(DetectableTarget target)
    {
        SetTarget(target.FollowTransform);
        _atRoute = false;
    }
}
