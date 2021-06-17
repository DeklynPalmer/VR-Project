using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerGun : MonoBehaviour
{
    public int m_MaxAmmoAmount = 6;
    public int m_CurrentAmmoAmount = 6;
    public int m_Damage = 20;

    public float m_TimeBetweenShots = 0.3f;
    private float m_TimeUntilNextShot = 0f;

    [Space]
    public float m_KnockbackPower = 50f;
    [Space]
    public float m_MinRecoilAngle = 8f;
    public float m_MaxRecoilAngle = 12f;
    public float m_MinKickbackDistance = 0.1f;
    public float m_MaxKickbackDistance = 0.25f;

    [Space]
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
            if (m_CurrentAmmoAmount > 0 && m_TimeUntilNextShot <= 0f && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, m_Controller))
            {
                Shoot();
                --m_CurrentAmmoAmount;
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

        /* Position the model back to its resting position */
        m_Model.position = Vector3.Lerp(m_Model.position, transform.position, 0.4f);
        m_Model.rotation = Quaternion.Lerp(m_Model.rotation, transform.rotation, 0.4f);
    }

    void Shoot()
    {
        Debug.Log("Shot");

        RaycastHit hit;
        if (Physics.Raycast(m_FirePoint.position, m_FirePoint.forward, out hit, 100f))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                /* Damage any hit enemy */
                hit.transform.GetComponent<Health>().DetachHealth(m_Damage);
            }
            else if (hit.transform.CompareTag("Throwable"))
            {
                /* Apply a force to any interactable object hit */
                hit.transform.GetComponent<Rigidbody>().AddForce((hit.point - m_FirePoint.position).normalized * m_KnockbackPower, ForceMode.Impulse);
            }
        }

        /* Add recoil to the model */
        m_Model.localPosition += Vector3.back * Random.Range(m_MinKickbackDistance, m_MaxKickbackDistance);
        //m_Model.rotation = Quaternion.Euler(m_Model.localRotation.eulerAngles + (Vector3.forward * Random.Range(m_MinRecoilAngle, m_MaxRecoilAngle));
    }

    public void AddAmmo(int amount)
    {
        m_CurrentAmmoAmount += amount;

        if (m_CurrentAmmoAmount > m_MaxAmmoAmount)
            m_CurrentAmmoAmount = m_MaxAmmoAmount;

        if (m_CurrentAmmoAmount < 0)
            m_CurrentAmmoAmount = 0;
    }
}
