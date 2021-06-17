using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private GameObject m_Player;
    private bool m_PlayerInside;

    private List<GameObject> m_Enemies = new List<GameObject>();
    private Dictionary<GameObject, bool> m_EnemiesInside = new Dictionary<GameObject, bool>();
    private List<GameObject> m_PlacableBoards = new List<GameObject>();

    [Header("Window Instances:")]
    [Tooltip("All of the Boards attached to the Window.")]
    public GameObject[] m_Boards;

    [Tooltip("The Bottom of the window.")]
    public BoxCollider m_BottomWindowCollider;

    [Tooltip("This is created when a board is removed.")]
    public GameObject m_BoardProp;

    private bool m_IsEmpty;
    private bool m_IsFull;

    void Update()
    {
        /* Constantly check if the Number of Boards is Empty. */
        int numberOfBoardsThatAreDeactivated = 0;
        for (int i = 0; i < m_Boards.Length; i++)
        {
            if (!m_Boards[i].activeSelf)
                numberOfBoardsThatAreDeactivated++;
        }

        /* See if it's Empty. */
        m_IsEmpty = (m_Boards.Length == numberOfBoardsThatAreDeactivated);
        m_IsFull = (numberOfBoardsThatAreDeactivated == 0);

        /* This is for AI Mainly. */
        if (m_IsEmpty)
            m_BottomWindowCollider.enabled = false;

        else
            m_BottomWindowCollider.enabled = true;
    }

    void OnTriggerStay(Collider collider)
    {
        Debug.Log(collider.name);

        /* Check if the collision is with the Player. */
        if (collider.gameObject == m_Player)
            m_PlayerInside = true;

        /* Go through the Enemies. */
        for(int i = 0; i < m_Enemies.Count; i++)
        {
            /* Check if it is one of the Enemies. */
            if (collider.gameObject == m_Enemies[i])
                m_EnemiesInside[m_Enemies[i]] = true;
        }

        /* Go through the Placable Boards. */
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            /* Check if it is one of the Boards. */
            if (collider.gameObject == m_PlacableBoards[i])
            {
                AttachBoard(m_PlacableBoards[i]);
                Destroy(m_PlacableBoards[i]);
                m_PlacableBoards.RemoveAt(i);

                break;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        /* Check if the collision is with the Player. */
        if (collider.gameObject == m_Player)
            m_PlayerInside = false;

        /* Go through the Enemies. */
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            /* Check if it is one of the Enemies. */
            if (collider.gameObject == m_Enemies[i])
                m_EnemiesInside[m_Enemies[i]] = false;
        }
    }

    /// <summary>
    /// Get's if all the boards are deactivated.
    /// </summary>
    /// <returns></returns>
    public bool IsWindowEmpty() { return m_IsEmpty; }

    /// <summary>
    /// Gets if the boards are all there.
    /// </summary>
    /// <returns></returns>
    public bool IsWindowFull() { return m_IsFull; }

    /// <summary>
    /// Attaches a board if availible.
    /// </summary>
    public void AttachBoard(GameObject gameObject)
    {
        /* Early out if Object is not acceptable. */
        if (!IsAllowedToInteract(gameObject))
            return;

        /* Early out if all boards are attached. */
        if (m_IsFull)
            return;
        
        /* Go through the Boards. */
        for(int i = m_Boards.Length; i >= 0; i--)
        {
            /* Check if they are Deactivated. */
            if (!m_Boards[i].activeSelf)
            {
                /* Re-activate it. */
                m_Boards[i].SetActive(true);
                break;
            }
        }
    }

    /// <summary>
    /// Detaches a board if avalible.
    /// </summary>
    public void DetachBoard(GameObject gameObject)
    {
        /* Early out if Object is not acceptable. */
        if (!IsAllowedToInteract(gameObject))
            return;

        /* Early out if there is no boars to attach. */
        if (m_IsEmpty)
            return;

        /* Go through the Boards. */
        for (int i = m_Boards.Length - 1; i >= 0; i--)
        {
            /* Check if they are Activated. */
            if (m_Boards[i].activeSelf)
            {
                /* Create a Dummy Board. */
                GameObject board = Instantiate(m_BoardProp, m_Boards[i].transform.position, m_Boards[i].transform.rotation);
                board.transform.Translate(new Vector3(0.0f, 0.25f, 0.0f));
                board.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 60.0f);

                /* Deactivate it. */
                m_Boards[i].SetActive(false);

                
                break;
            }
        }
    }

    /// <summary>
    /// Checks if the Object is allowed to interact with the Window.
    /// </summary>
    /// <param name="gameObject">The Object passed in.</param>
    /// <returns></returns>
    public bool IsAllowedToInteract(GameObject gameObject)
    {
        /* Check if its the Player. */
        if (gameObject == m_Player && m_PlayerInside)
            return true;

        /* Go throug the Enemy list. */
        for(int i = 0; i < m_Enemies.Count; i++)
        {
            /* If the Object is an Enemy. */
            if (gameObject == m_Enemies[i] && m_EnemiesInside[gameObject])
                return true;
        }

        /* Go through the placable boards. */
        for(int i = 0; i < m_PlacableBoards.Count; i++)
        {
            /* if its a board we are looking for. */
            if (gameObject == m_PlacableBoards[i])
                return true;
        }

        /* If all else fails, not allowed. */
        return false;
    }

    /// <summary>
    /// Sets the Player Instance.
    /// </summary>
    /// <param name="player"></param>
    public void SetPlayer(GameObject player)
    {
        /* Sets the Player Instance. */
        m_Player = player;
    }

    /// <summary>
    /// Attaches an Enemy to the Manager to allow it to interact with the Boards.
    /// </summary>
    /// <param name="gameObject"></param>
    public void AttachEnemy(GameObject gameObject)
    {
        /* Adds an Enemy to the List and Dictionary. */
        m_Enemies.Add(gameObject);
        m_EnemiesInside.Add(gameObject, false);
    }

    /// <summary>
    /// Detaches an Enemy from the Manager so it doesn't interact with the Boards.
    /// </summary>
    /// <param name="gameObject"></param>
    public void DetachEnemy(GameObject gameObject)
    {
        /* Remove and Enemy from the List and Dictionary. */
        m_EnemiesInside.Remove(gameObject);
        m_Enemies.Remove(gameObject);
    }

    /// <summary>
    /// Attaches a Placable Board.
    /// </summary>
    /// <param name="gameObject"></param>
    public void AttachPlacableBoard(GameObject gameObject)
    {
        /* Add the board. */
        m_PlacableBoards.Add(gameObject);
    }
}
