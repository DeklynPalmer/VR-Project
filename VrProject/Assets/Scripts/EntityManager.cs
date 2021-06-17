using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [Header("Player Entity:")]
    [Tooltip("Player GameObject.")]
    public GameObject m_Player;

    [Header("Enemy Entity:")]
    public GameObject m_Alien;

    [Header("Alien Spawn Points:")]
    public Transform[] m_AlienSpawnpoints;

    [Header("Alien Spawn Timings/Cappings:")]
    [Tooltip("The maximum aliens to be spawned at once.")]
    public int m_MaxAlienSpawnCount = 20;
    private int m_AlienSpawnCount;

    [Tooltip("In Seconds.")]
    public float m_AlienSpawnTime = 2.0f;
    private float m_MaxAlienSpawnTime;
    
    [Header("Board Managers:")]
    public BoardManager[] m_BoardManagers;

    [Header("Board Spawn Points:")]
    public Transform[] m_BoardSpawnpoints;
    private Dictionary<Transform, GameObject> m_BoardSpawnCache;

    [Header("Board Cap:")]
    [Tooltip("The maximum boards to be spawned at once.")]
    public int m_MaxBoardSpawnCount = 3;
    private int m_BoardSpawnCount;

    [Header("Board Object:")]
    public GameObject m_Board;

    void Start()
    {
        /* Set the Max Spawn Time. */
        m_MaxAlienSpawnTime = m_AlienSpawnTime;

        /* Set the Player Instance at Start. */
        for(int i = 0; i < m_BoardManagers.Length; i++)
            m_BoardManagers[i].SetPlayer(m_Player);

        m_BoardSpawnCache = new Dictionary<Transform, GameObject>();

        /* Initialise the Board Cache. */
        for (int i = 0; i < m_BoardSpawnpoints.Length; i++)
            m_BoardSpawnCache.Add(m_BoardSpawnpoints[i], null);

        /* Spawn all of the Boards. */
        while (m_BoardSpawnCount != m_MaxBoardSpawnCount)
        {
            /* Grab a random spawn. */
            int index = Random.Range(0, m_BoardSpawnpoints.Length);

            /* If the Board Cache returns null for the selected spawn point. */
            if(!m_BoardSpawnCache[m_BoardSpawnpoints[index]])
            {
                /* Create an Instance at the Board Location. */
                GameObject board = Instantiate(m_Board, m_BoardSpawnpoints[index].position, m_BoardSpawnpoints[index].rotation);

                /* Reference the Entity Manager. */
                board.GetComponent<Board>().SetEntityManager(this);

                /* Go through the Board Managers. */
                for(int i = 0; i <m_BoardManagers.Length; i++)
                {
                    /* Attach the Board. */
                    m_BoardManagers[i].AttachPlacableBoard(board);
                }

                /* Cache it. */
                m_BoardSpawnCache[m_BoardSpawnpoints[index]] = board;

                /* Increase the Spawn Count. */
                m_BoardSpawnCount++;
            }
        }
    }

    void Update()
    {
        if(m_AlienSpawnCount < m_MaxAlienSpawnCount)
        {
            if (m_AlienSpawnTime < m_MaxAlienSpawnTime)
                m_AlienSpawnTime += Time.deltaTime;

            else
            {
                /* Grab a random spawn. */
                int index = Random.Range(0, m_AlienSpawnpoints.Length);

                /* Create an Instance at the Board Location. */
                GameObject alien = Instantiate(m_Alien, m_AlienSpawnpoints[index].position, m_AlienSpawnpoints[index].rotation);

                /* Set Alien Targets. */
                Alien alienScript = alien.GetComponent<Alien>();
                alienScript.m_Managers = m_BoardManagers;
                alienScript.m_Player = m_Player.transform;

                /* Attach this Enemy to the Board Managers. */
                for (int i = 0; i < m_BoardManagers.Length; i++)
                    m_BoardManagers[i].AttachEnemy(alien);

                /* Reset. */
                m_AlienSpawnTime = 0.0f;
                m_AlienSpawnCount++;
            }
        }
        /* If we don't have the max number of boards in play. */
        if(m_BoardSpawnCount < m_MaxBoardSpawnCount)
        {
            /* Spawn the Boards. */
            while (m_BoardSpawnCount != m_MaxBoardSpawnCount)
            {
                /* Grab a random spawn. */
                int index = Random.Range(0, m_BoardSpawnpoints.Length);

                /* If the Board Cache returns null for the selected spawn point. */
                if (!m_BoardSpawnCache[m_BoardSpawnpoints[index]])
                {
                    /* Create an Instance at the Board Location. */
                    GameObject board = Instantiate(m_Board, m_BoardSpawnpoints[index].position, m_BoardSpawnpoints[index].rotation);

                    /* Reference the Entity Manager. */
                    board.GetComponent<Board>().SetEntityManager(this);

                    /* Go through the Board Managers. */
                    for (int i = 0; i < m_BoardManagers.Length; i++)
                    {
                        /* Attach the Board. */
                        m_BoardManagers[i].AttachPlacableBoard(board);
                    }

                    /* Cache it. */
                    m_BoardSpawnCache[m_BoardSpawnpoints[index]] = board;

                    /* Increase the Spawn Count. */
                    m_BoardSpawnCount++;
                }
            }
        }
    }

    /// <summary>
    /// Removes a board from the Cache.
    /// </summary>
    /// <param name="board"></param>
    public void RemoveBoard(GameObject board)
    {
        /* Go through all Spawnpoints. */
        for (int i = 0; i < m_BoardSpawnpoints.Length; i++)
        {
            /* If the Board belongs to one of the Transforms, null it. */ 
            if (m_BoardSpawnCache[m_BoardSpawnpoints[i]] == board)
            {
                /* Null it out. */
                m_BoardSpawnCache[m_BoardSpawnpoints[i]] = null;

                /* Decrease the Count. */
                m_BoardSpawnCount--;

                break;
            }
        }
    }
}
