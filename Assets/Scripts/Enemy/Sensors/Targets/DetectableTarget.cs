using UnityEngine;

public class DetectableTarget : MonoBehaviour
{
    public Transform FollowTransform;

    private void Start()
    {
        TargetsManager.instance.Register(this);
    }
    
    private void OnEnable()
    {
        TargetsManager.instance.Register(this);
    }

    private void OnDestroy()
    {
        TargetsManager.instance.Deregister(this);
    }

    private void OnDisable()
    {
        TargetsManager.instance.Deregister(this);
    }
}
