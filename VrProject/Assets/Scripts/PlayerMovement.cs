using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    public float m_PlayerHeight = 1.5f;
    public float m_MoveSpeed = 180f;

    public Transform m_Head;
    public Transform m_BaseCollider;

    private Vector3 m_MoveDir;

    private Rigidbody m_RB;

    void Start()
    {
        m_RB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        m_MoveDir = GetMoveDirection();

        m_RB.velocity = m_MoveDir * m_MoveSpeed * Time.deltaTime;

        m_PlayerHeight = transform.localPosition.y + m_Head.localPosition.y;

        m_BaseCollider.position = m_Head.position - (Vector3.up * (m_PlayerHeight - 0.1f));
    }

    /// <summary>
    /// Returns the desired move direction based on the left controllers input
    /// </summary>
    Vector3 GetMoveDirection()
    {
        Vector2 inputDir = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
        Vector3 forwardDir = Quaternion.Euler(0, m_Head.rotation.eulerAngles.y, 0) * Vector3.forward;
        Vector3 rightDir = Quaternion.Euler(0, 90, 0) * forwardDir;
        return (forwardDir * inputDir.y + rightDir * inputDir.x).normalized;
    }


}
