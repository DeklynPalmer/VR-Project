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

    [Header("Timings:")]
    [Tooltip("In Seconds.")]
    public float m_AttackTime = 3.0f;
    private float m_MaxAttackTime;

    private BoardManager m_TargetWindow;
    private NavMeshAgent m_Agent;

    void Start()
    {
        /* Set the Max Attack Time. */
        m_MaxAttackTime = m_AttackTime;

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

    // Update is called once per frame
    void Update()
    {
        /* If the Window isn't Empty. */
        if(!m_TargetWindow.IsWindowEmpty() && m_TargetWindow)
        {
            /* If the Attack Time hasn't fully elapsed. */
            if (m_AttackTime < m_MaxAttackTime)
                m_AttackTime += Time.deltaTime;

            /* If it has. */
            else
            {
                /* Set the Proper Targets. */
                m_TargetWindow.DetachBoard(gameObject);
                m_Agent.SetDestination(m_TargetWindow.transform.position);

                /* Reset the Attack Time. */
                m_AttackTime = 0.0f;
            }
        }

        /* If the Window is Empty. */
        else
        {
            /* Move onto the Player. */
            m_Agent.SetDestination(m_Player.position);

            /* No Need to focus on the Window. */
            m_TargetWindow = null;
        }
    }
}
