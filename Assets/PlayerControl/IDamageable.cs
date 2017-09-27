using UnityEngine;
using System.Collections;

public interface IDamageable
{
    int StartingHealth { get; }
    int Health { get; }
    bool Dead { get; }

    void TakeHit(int damage, Vector3 hitPoint, Vector3 hitDirection);
    void TakeDamage(int damage);
    void Die();
}
