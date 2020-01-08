using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float totalHealth = 100;

    [HideInInspector]
    public float currentHealth;

    [HideInInspector]
    public bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = totalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
    }

    private void Die()
    {
        dead = true;
    }
}
