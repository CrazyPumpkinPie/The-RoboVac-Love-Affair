using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EOffmeshLinkStatus
{
    NotStarted,
    InProgress
}

[RequireComponent(typeof(NavMeshAgent))]
public class EntityMotor : Entity_Base
{
    [SerializeField] float _nearestPointSearchRange = 5f;

    NavMeshAgent _agent;
    bool _destinationSet = false;
    bool _reachedDestination = false;
    EOffmeshLinkStatus _offMeshLinkStatus = EOffmeshLinkStatus.NotStarted;

    public bool IsMoving => _agent.velocity.magnitude > float.Epsilon;

    public bool AtDestination => _reachedDestination;

    protected void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    protected void Update()
    {
        // have a path and near the end point?
        if (!_agent.pathPending && !_agent.isOnOffMeshLink && _destinationSet && (_agent.remainingDistance <= _agent.stoppingDistance))
        {
            _destinationSet = false;
            _reachedDestination = true;
        }

        // are we on an offmesh link?
        if (_agent.isOnOffMeshLink)
        {
            // have we started moving along the link
            if (_offMeshLinkStatus == EOffmeshLinkStatus.NotStarted)
                StartCoroutine(FollowOffmeshLink());
        }
    }

    IEnumerator FollowOffmeshLink()
    {
        // start the offmesh link - disable NavMesh agent control
        _offMeshLinkStatus = EOffmeshLinkStatus.InProgress;
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        // move along the path
        Vector3 newPosition = transform.position;
        while (!Mathf.Approximately(Vector3.Distance(newPosition, _agent.currentOffMeshLinkData.endPos), 0f))
        {
            newPosition = Vector3.MoveTowards(transform.position, _agent.currentOffMeshLinkData.endPos, _agent.speed * Time.deltaTime);
            transform.position = newPosition;

            yield return new WaitForEndOfFrame();
        }

        // flag the link as completed
        _offMeshLinkStatus = EOffmeshLinkStatus.NotStarted;
        _agent.CompleteOffMeshLink();

        // return control the agent
        _agent.updatePosition = true;
        _agent.updateRotation = true;
        _agent.updateUpAxis = true;
    }

    public Vector3 PickLocationInRange(float range)
    {
        Vector3 searchLocation = transform.position;
        searchLocation += Random.Range(-range, range) * Vector3.forward;
        searchLocation += Random.Range(-range, range) * Vector3.right;

        NavMeshHit hitResult;
        if (NavMesh.SamplePosition(searchLocation, out hitResult, _nearestPointSearchRange, NavMesh.AllAreas))
            return hitResult.position;

        return transform.position;
    }

    protected virtual void CancelCurrentCommand()
    {
        // clear the current path
        _agent.ResetPath();

        _destinationSet = false;
        _reachedDestination = false;
        _offMeshLinkStatus = EOffmeshLinkStatus.NotStarted;
    }

    public virtual void MoveTo(Vector3 destination)
    {
        CancelCurrentCommand();

        SetDestination(destination);
    }

    public virtual void SetDestination(Vector3 destination)
    {
        // find nearest spot on navmesh and move there
        NavMeshHit hitResult;
        if (NavMesh.SamplePosition(destination, out hitResult, _nearestPointSearchRange, NavMesh.AllAreas))
        {
            _agent.SetDestination(hitResult.position);
            _destinationSet = true;
            _reachedDestination = false;
        }
    }
}