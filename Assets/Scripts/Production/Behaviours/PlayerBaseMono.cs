using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseMono : MonoBehaviour, IHealth
{

    [SerializeField]
    private int m_MaxHealth;

    [SerializeField]
    private int currentHealth;

    private Rigidbody rb;
    private bool dead;

    public bool Dead { get { return dead; } set { dead = value; } }
    private void Start()
    {
        currentHealth = m_MaxHealth;
        
    }

    public void Damage(int damageTaken)
    {
        currentHealth -= damageTaken;
        if (currentHealth <= 0) { dead = true; }
    }
}
