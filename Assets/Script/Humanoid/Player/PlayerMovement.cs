using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Vector3 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (direction.magnitude > 0.4f)
        {
            rb.velocity = direction.normalized * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            rb.velocity = Vector3.zero;
            Debug.Log("Obstacle");
        }
    }
}
