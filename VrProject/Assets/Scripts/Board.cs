using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private EntityManager m_Manager;

    void OnDestroy()
    {
        /* Remove the Board. */
        m_Manager.RemoveBoard(gameObject);
    }

    /// <summary>
    /// Adds a Reference to the Entity Manager.
    /// </summary>
    /// <param name="manager"></param>
    public void SetEntityManager(EntityManager manager)
    {
        m_Manager = manager;
    }
}
