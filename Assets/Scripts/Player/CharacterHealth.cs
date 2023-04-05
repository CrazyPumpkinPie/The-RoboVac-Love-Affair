using UnityEngine;
using UnityEngine.Events;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] int _healthSize = 3;
    [Space]
    [SerializeField] float _invincibilityTimeOnDamaged = .2f;
    public bool IsInvincible = false;
    [Space]
    [SerializeField] UnityEvent OnDeath;

    int _health;

    float _invincibilityTimer = 0f;

    private void Awake()
    {
        _health = _healthSize;
    }

    private void Update()
    {
        _invincibilityTimer -= Mathf.Min(_invincibilityTimer, Time.deltaTime);

        if (_invincibilityTimer > 0f)
            IsInvincible = true;
        else
            IsInvincible = false;
    }

    public void ApplyDamage(int amount)
    {
        if (IsInvincible)
            return;

        _health -= amount;
        _invincibilityTimer += _invincibilityTimeOnDamaged;

        if (_health <= 0)
            Die();
    }

    void Die()
    {
        gameObject.SetActive(false);

        OnDeath?.Invoke();
    }
}
