using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class VisionSensor : MonoBehaviour
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
            if (vectorToTarget.sqrMagnitude > (AI.SightRange * AI.SightRange)) continue;

            vectorToTarget.Normalize();

            // skip if out of field of view 
            if (Vector3.Dot(vectorToTarget, AI.Eyes.forward) < AI.CosViewAngle) continue;

            // raycast
            RaycastHit hit;
            if (Physics.Raycast(AI.Eyes.position, vectorToTarget, out hit, AI.SightRange, DetectionMask, QueryTriggerInteraction.Collide))
            {
                //var IndexCheck = Array.Find(Index, i => i == target.TargetIndex);
                if (hit.collider.gameObject.GetComponentInParent<DetectableTarget>() == target/** && IndexCheck == target.TargetIndex**/) AI.ReportTarget(target);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AI.Eyes.position, AI.SightRange);
    }
}
