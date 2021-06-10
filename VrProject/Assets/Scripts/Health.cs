using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Values:")]
    public int m_Health = 100;
    private int m_MaxHealth;

    private bool m_IsDead = false;

    void Start()
    {
        /* Set the Max health. */
        m_MaxHealth = m_Health;
    }

    void Update()
    {
        /* If not Dead. */
        if (!IsDead())
        {
            /* If the Health is above the Max. */
            if (m_Health > m_MaxHealth)
                m_Health = m_MaxHealth; /* Set Health to Max. */

            /* If the Health is or below Zero. */
            if (m_Health <= 0)
            {
                /* We dieded. */
                m_IsDead = true;
                m_Health = 0;
            }
        }
    }

    /// <summary>
    /// Sets the Health.
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(int health)
    {
        /* Set the Health Values. */
        m_Health = health;
        m_MaxHealth = health;
    }

    /// <summary>
    /// Gets Health.
    /// </summary>
    /// <returns></returns>
    public int GetHealth()
    {
        /* Return the Health Value. */
        return m_Health;
    }

    /// <summary>
    /// Gets Max Health.
    /// </summary>
    /// <returns></returns>
    public int GetMaxHealth()
    {
        /* Returns the Max Health Value. */
        return m_MaxHealth;
    }

    /// <summary>
    /// Adds Health.
    /// </summary>
    /// <param name="amount"></param>
    public void AttachHealth(int amount)
    {
        /* Add the Amount to the Health. */
        m_Health += amount;
    }

    /// <summary>
    /// Removes health.
    /// </summary>
    /// <param name="amount"></param>
    public void DetachHealth(int amount)
    {
        /* Remove the amount from the Health. */
        m_Health -= amount;
    }

    /// <summary>
    /// Gets if the Object is dead.
    /// </summary>
    /// <returns></returns>
    public bool IsDead() 
    {
        /* Returns if we are dead. */
        return m_IsDead; 
    }
}
