using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunLoader : MonoBehaviour
{
    public int m_AmmoAmount = 6;

    public PlayerGun m_PlayerGun;
    public ObjectInteractions m_ObjInteractions;

    [Space]
    public Transform m_HolsterPos;

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
        }
        else
        {
            /* Position to the holster */
            transform.position = Vector3.Lerp(transform.position, m_HolsterPos.position, 0.5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, m_HolsterPos.rotation, 0.5f);

            /* Disable physics */
            m_RB.isKinematic = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "GunChamberTrigger")
        {
            m_PlayerGun.AddAmmo(m_AmmoAmount);

            m_IsHeld = false;
            transform.position = m_HolsterPos.position;
            transform.rotation = m_HolsterPos.rotation;
            m_RB.isKinematic = true;

            m_ObjInteractions.DropHeldObject(m_Controller);
        }
    }
}
