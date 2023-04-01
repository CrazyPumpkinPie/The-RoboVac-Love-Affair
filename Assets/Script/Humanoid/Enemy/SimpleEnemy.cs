using System;
using UnityEngine;

public class SimpleEnemy : TamplateEnemy
{
    [SerializeField] private Rigidbody2D EnemyRb;
    private void Start()
    {
        chooseDestination();
        EnemyRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {   
        EnemyRb.velocity = target.direction * walkSpeed;
        Vector2 distance = target.destination.position - transform.position;

        if (distance.magnitude <= 0.1f)
        {
            chooseDestination();
        }
    }
    protected override void chooseDestination()
    {
        Transform currentDestination;
        do
        {
            currentDestination = PointToPatroll[Convert.ToInt32(UnityEngine.Random.Range(0, PointToPatroll.Count))];
            target.direction = new Vector2(currentDestination.position.x - transform.position.x, currentDestination.position.y - transform.position.y).normalized;
        }
        while (target.destination != currentDestination);

        target.destination = currentDestination;
    }
}
