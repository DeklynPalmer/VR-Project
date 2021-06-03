using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [Header("Player Entity:")]
    [Tooltip("Player GameObject.")]
    public GameObject m_Player;

    [Header("Board Managers:")]
    public BoardManager[] m_BoardManagers;

    void Start()
    {
        /* Set the Player Instance at Start. */
        for(int i = 0; i < m_BoardManagers.Length; i++)
            m_BoardManagers[i].SetPlayer(m_Player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
