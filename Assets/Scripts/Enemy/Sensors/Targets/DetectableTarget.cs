using UnityEngine;

public class DetectableTarget : MonoBehaviour
{
    public Transform FollowTransform;

    private void Start()
    {
        TargetsManager.instance.Register(this);
    }

    private void OnDestroy()
    {
        TargetsManager.instance.Deregister(this);
    }
}
