using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Alien : MonoBehaviour
{
    [Header("Player Position:")]
    public Transform m_Player;

    [Header("Windows:")]
    public BoardManager[] m_Managers;

    [Header("Animator:")]
    public Animator m_Animator;

    [Header("Timings:")]
    [Tooltip("In Seconds.")]
    public float m_AttackTime = 3.0f;
    private float m_MaxAttackTime;

    [Header("Damage Values:")]
    public int m_AttackDamage = 10;

    [Header("Effects:")]
    public ParticleSystem m_HitEffect;

    private BoardManager m_TargetWindow;
    private NavMeshAgent m_Agent;
    private Health m_Health;

    void Start()
    {
        /* Set the Max Attack Time. */
        m_MaxAttackTime = m_AttackTime;

        /* Get the Health Script. */
        m_Health = GetComponent<Health>();

        /* Get the Navagation Mesh Agent. */
        m_Agent = GetComponent<NavMeshAgent>();

        /* Default this to a ridiculous value to allow it to search the closest window. */
        float closestDistance = float.MaxValue;

        /* Go through each Window. */
        for(int i = 0; i < m_Managers.Length; i++)
        {
            /* Get the distance from the AI to the Window. */
            float distance = Vector3.Distance(transform.position, m_Managers[i].transform.position);

            /* If the Distance to that window is smaller than the current closest distance. */
            if (distance < closestDistance)
            {
                /* Set values. */
                m_TargetWindow = m_Managers[i];
                closestDistance = distance;
            }
        }
    }

    void Update()
    {
        /* If the Alien is Alive. */
        if (!m_Health.IsDead())
        {
            if (m_TargetWindow)
            {
                m_Agent.SetDestination(m_TargetWindow.transform.position);

                /* Get the distance from the AI to the Window. */
                float distance = Vector3.Distance(transform.position, m_TargetWindow.transform.position);

                Debug.Log(distance + " " + m_Agent.stoppingDistance);

                if (distance <= m_Agent.stoppingDistance + 0.25f)
                {
                    /* If the Window isn't Empty. */
                    if (!m_TargetWindow.IsWindowEmpty())
                    {
                        m_Animator.SetBool("isAttacking", true);

                        /* If the Attack Time hasn't fully elapsed. */
                        if (m_AttackTime < m_MaxAttackTime)
                            m_AttackTime += Time.deltaTime;

                        /* If it has. */
                        else
                        {
                            /* Set the Proper Targets. */
                            m_TargetWindow.DetachBoard(gameObject);

                            /* Reset the Attack Time. */
                            m_AttackTime = 0.0f;
                        }
                    }

                    /* If the Window is Empty. */
                    else
                    {
                        m_Animator.SetBool("isAttacking", false);
                        /* No Need to focus on the Window. */
                        m_TargetWindow = null;
                    }
                }
            }

            else
            {
                /* Move onto the Player. */
                m_Agent.SetDestination(m_Player.position);
            }
        }

        else
        {
            /* if we have a particle effect. */
            if(m_HitEffect)
            {
                /* If we aren't playing the hit effect. */
                if(!m_HitEffect.isPlaying)
                {
                    ParticleSystem.MainModule module = m_HitEffect.main;
                    module.startSpeed = 2.0f;
                    m_HitEffect.Play();
                }
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        /* If the Collider that has entered the box trigger is attached to the Player. */
        if (other.gameObject == m_Player.gameObject)
        {
            /* If we aren't targeting the window. */
            if(!m_TargetWindow)
            {
                Debug.Log("Trying to Attack the Player.");

                m_Animator.SetBool("isAttacking", true);

                /* If the Attack Time hasn't fully elapsed. */
                if (m_AttackTime < m_MaxAttackTime)
                    m_AttackTime += Time.deltaTime;

                /* If it has. */
                else
                {
                    Debug.Log("Attacked the Player.");
                    /* Damage the Player. */
                    Health playerHealth = m_Player.GetComponent<Health>();
                    playerHealth.DetachHealth(m_AttackDamage);

                    /* Reset the Attack Time. */
                    m_AttackTime = 0.0f;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /* If the Collider that has entered the box trigger is attached to the Player. */
        if (other.gameObject == m_Player)
        {
            /* If we aren't targeting the window. */
            if (!m_TargetWindow)
            {
                m_Animator.SetBool("isAttacking", false);
                /* Reset the Attack Timer. */
                m_AttackTime = 0.0f;
            }
        }
    }
}
