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
    private void Start()
    {
        currentHealth = m_MaxHealth;
        
    }

    public void Damage(int damageTaken)
    {
        currentHealth -= damageTaken;
    }
}
