using UnityEngine;

public class ProximitySensor : MonoBehaviour
{
    [SerializeField] LayerMask DetectionMask;
    //[SerializeField] int[] Index = { 0 };// the sensor will be triggered only to targets with this index

    EnemyAI AI;

    public void Start()
    {
        AI = GetComponent<EnemyAI>();
    }

    void Update()
    {
        // check all possible targets
        for (int i = 0; i < TargetsManager.instance.Targets.Count; ++i)
        {
            var target = TargetsManager.instance.Targets[i];

            // skip if target is ourselves
            if (target.gameObject == gameObject) continue;

            var vectorToTarget = target.transform.position - AI.Eyes.position;

            // skip if out of range
            if (vectorToTarget.magnitude > (AI.SightRange)) continue;

            AI.ReportTarget(target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (AI == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AI.Eyes.position, AI.SightRange);
    }
}
