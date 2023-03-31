using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float speed;
    private Vector3 target;
    private void Start()
    {
        target = transform.position - playerTransform.position;
    }
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + target, Time.deltaTime * speed);
    }
}
