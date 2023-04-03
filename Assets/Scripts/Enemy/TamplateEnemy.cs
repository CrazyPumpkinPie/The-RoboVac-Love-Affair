using System.Collections.Generic;
using UnityEngine;

public abstract class TamplateEnemy : MonoBehaviour
{
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;
    [SerializeField] protected float culldawn;
    [SerializeField] protected float distanceToChase;
    [SerializeField] protected Transform player;
    [SerializeField] protected List<Transform> PointToPatroll;
    private protected Target target = new Target();

    protected abstract void chooseDestination();

    public class Target
    {
        public Transform destination;
        public Vector2 direction;
    }
}