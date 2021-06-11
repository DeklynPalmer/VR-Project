using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerGun : MonoBehaviour
{
    public float m_Damage = 20f;
    public int m_Ammo = 6;

    public float m_TimeBetweenShots = 0.3f;
    private float m_TimeUntilNextShot = 0f;

    public Transform m_Model;
    public Transform m_FirePoint;

    [Space]
    public bool m_IsHeld;
    public OVRInput.Controller m_Controller;

    void Update()
    {
        if (m_IsHeld && m_Ammo > 0 && m_TimeUntilNextShot <= 0f && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, m_Controller))
        {
            Shoot();
            --m_Ammo;
            m_TimeUntilNextShot = m_TimeBetweenShots;
        }
        else if (m_TimeUntilNextShot > 0f)
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
                //hit.transform.GetComponent<Enemy>().TakeDamage(m_Damage);
            }
        }
    }
}
