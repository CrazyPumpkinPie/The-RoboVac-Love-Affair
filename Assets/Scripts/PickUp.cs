using UnityEngine;
using UnityEngine.Events;

public class PickUp : MonoBehaviour
{
    [SerializeField] UnityEvent _onPicked;

    private void OnTriggerEnter(Collider other)
    {
        Pick();
    }

    void Pick()
    {
        _onPicked?.Invoke();

        SelfDestroy();
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
