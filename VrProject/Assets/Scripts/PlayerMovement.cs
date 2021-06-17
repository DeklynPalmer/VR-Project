using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    public float m_PlayerHeight = 1.5f;
    public float m_MoveSpeed = 180f;
    public float m_TurnSpeed = 180f;

    public Transform m_Head;
    public Transform m_BaseCollider;
    public Transform m_Holster;

    private Vector3 m_MoveDir;
    Vector3 m_2DForwardDir;

    private Rigidbody m_RB;

    void Start()
    {
        m_RB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        m_2DForwardDir = Quaternion.Euler(0, m_Head.rotation.eulerAngles.y, 0) * Vector3.forward;

        m_MoveDir = GetMoveDirection();

        /* Movement */
        m_RB.velocity = m_MoveDir * m_MoveSpeed * Time.deltaTime;

        /* Get the players height */
        //m_PlayerHeight = transform.localPosition.y + m_Head.localPosition.y;
        m_PlayerHeight = m_Head.localPosition.y;

        /* Position the base collider */
        m_BaseCollider.position = m_Head.position - (Vector3.up * (m_PlayerHeight - 0.1f));

        /* Position the holster */
        m_Holster.position = m_Head.position - (Vector3.up * (m_PlayerHeight * 0.5f));
        m_Holster.rotation = Quaternion.LookRotation(m_2DForwardDir, Vector3.up);

        /* Turn the player */
        m_RB.rotation = Quaternion.Euler(0, m_RB.rotation.eulerAngles.y + (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x * m_TurnSpeed * Time.deltaTime), 0);
    }

    /// <summary>
    /// Returns the desired move direction based on the left controllers input
    /// </summary>
    Vector3 GetMoveDirection()
    {
        Vector2 inputDir = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
        //Vector3 forwardDir = Quaternion.Euler(0, m_Head.rotation.eulerAngles.y, 0) * Vector3.forward;
        Vector3 rightDir = Quaternion.Euler(0, 90, 0) * m_2DForwardDir;
        return (m_2DForwardDir * inputDir.y + rightDir * inputDir.x).normalized;
    }


}
