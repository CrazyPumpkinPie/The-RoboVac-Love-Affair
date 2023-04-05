using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float _attackRange = .3f;
    [SerializeField] Transform _attackPoint;
    [SerializeField] int _damage = 1;
    [SerializeField] float _cooldown = .3f;

    float _cooldownTimer = 0f;

    private void Update()
    {
        _cooldownTimer -= Mathf.Min(_cooldownTimer, Time.deltaTime);

        if (TargetsManager.instance.Targets.Count <= 0 || _cooldownTimer > 0f)
            return;

        for (int i = 0; i < TargetsManager.instance.Targets.Count; i++)
        {
            var target = TargetsManager.instance.Targets[i];

            if (Vector3.Distance(_attackPoint.position, target.FollowTransform.position) > _attackRange)
                continue;

            var health = target.gameObject.GetComponent<CharacterHealth>();

            if (health != null)
                Attack(health);
        }
    }

    void Attack(CharacterHealth health)
    {
        _cooldownTimer += _cooldown;

        health.ApplyDamage(_damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
}
