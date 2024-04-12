using System;

public interface IDamageable
{
    int Health { get; set; }

    void TakeDamage(int damage);

    Action<int> OnHealthChanged { get; set; }

    void Die();
}