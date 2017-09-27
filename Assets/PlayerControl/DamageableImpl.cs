using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableImpl : MonoBehaviour, IDamageable
{
    [SerializeField] private int _startingHealth = 100;

    public int StartingHealth { get { return _startingHealth; } }
    public int Health { get; private set; }
    public bool Dead { get; private set; }

    public Action<int> OnDamaged;
    public Action OnDeath;

    private void Start()
    {
        Health = StartingHealth;
    }

    public void Die()
    {
        Dead = true;
        if (OnDeath != null)
        {
            OnDeath();
            OnDeath = null;
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        Debug.Log(gameObject.name);
        if (Health <= 0 && !Dead)
        {
            Die();
        }
    }

    public void TakeHit(int damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage);

        if (OnDamaged != null)
        {
            OnDamaged(damage);
        }
    }
}
