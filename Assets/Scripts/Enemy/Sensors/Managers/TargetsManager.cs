using System.Collections.Generic;
using UnityEngine;

public class TargetsManager : MonoBehaviour
{
    public static TargetsManager instance { get; private set; } = null;

    public List<DetectableTarget> Targets { get; private set; } = new List<DetectableTarget>();

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning($"More than one instance of {this} found on {gameObject.name}");

        instance = this;
    }

    public void Register(DetectableTarget target)
    {
        Targets.Add(target);
    }

    public void Deregister(DetectableTarget target)
    {
        Targets.Remove(target);
    }
}
