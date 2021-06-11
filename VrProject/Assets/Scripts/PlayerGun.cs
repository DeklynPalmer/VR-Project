using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerGun : MonoBehaviour
{
    public int m_Damage = 20;
    public int m_Ammo = 6;

    public float m_TimeBetweenShots = 0.3f;
    private float m_TimeUntilNextShot = 0f;

    public Transform m_Model;
    public Transform m_HolsterPos;
    public Transform m_FirePoint;

    [HideInInspector]
    public bool m_IsHeld;
    [HideInInspector]
    public OVRInput.Controller m_Controller;

    private Rigidbody m_RB;

    void Start()
    {
        m_RB = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (m_IsHeld)
        {
            /* Enable physics */
            m_RB.isKinematic = false;

            /* Fire the gun */
            if (m_Ammo > 0 && m_TimeUntilNextShot <= 0f && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, m_Controller))
            {
                Shoot();
                --m_Ammo;
                m_TimeUntilNextShot = m_TimeBetweenShots;
            }
        }
        else
        {
            /* Position to the holster */
            transform.position = Vector3.Lerp(transform.position, m_HolsterPos.position, 0.5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, m_HolsterPos.rotation, 0.5f);

            /* Disable physics */
            m_RB.isKinematic = true;
        }

        if (m_TimeUntilNextShot > 0f)
        {
            m_TimeUntilNextShot -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        Debug.Log("Shot");

        RaycastHit hit;
        if (Physics.Raycast(m_FirePoint.position, m_FirePoint.forward, out hit, 100f))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<Health>().DetachHealth(m_Damage);
            }
        }
    }
}
