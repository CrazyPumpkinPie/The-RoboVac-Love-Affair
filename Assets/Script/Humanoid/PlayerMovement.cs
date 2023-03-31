using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), rb.velocity.y, Input.GetAxis("Vertical"));
        rb.velocity = direction.normalized * speed * Time.fixedDeltaTime;
    }
}
