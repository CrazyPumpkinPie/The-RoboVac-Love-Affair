using UnityEngine;

public class aquaChan : MonoBehaviour
{
    [SerializeField] private float intakeSpeed = .2f;
    [SerializeField] private float minSize = 0.2f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            float f = intakeSpeed * Time.deltaTime;
            transform.localScale -= new Vector3(f, f, f);
            if(transform.localScale.x <= minSize)
            {
                Destroy(gameObject);
            }
        }
    }
}
