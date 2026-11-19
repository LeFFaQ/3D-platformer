using System;
using UnityEngine;

public sealed class CharacterHealth : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxHealth = 5;

    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    public event Action<int> HealthChanged;
    public event Action Died;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void Hit(int damage)
    {
        if (IsDead || damage <= 0)
            return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        HealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth == 0)
        {
            IsDead = true;
            Died?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (IsDead || amount <= 0)
            return;

        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        HealthChanged?.Invoke(CurrentHealth);
    }

    public void ResetHealth()
    {
        IsDead = false;
        CurrentHealth = maxHealth;
        HealthChanged?.Invoke(CurrentHealth);
    }
}