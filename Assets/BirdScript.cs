using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    [Tooltip("RollingStone speed.")]
    [SerializeField] float moveHorizontal;


    [Tooltip("RollingStone object")]
    [SerializeField] Rigidbody2D bird;

    private void Awake()
    {
        bird = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        bird.velocity = new Vector2(moveHorizontal, 0);
    }
}
