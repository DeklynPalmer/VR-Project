using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectInteractions : MonoBehaviour
{

    public Transform m_LeftHand, m_RightHand;
    private Rigidbody m_LeftGrabbedObject, m_RightGrabbedObject;

    private const float grabDistance = 0.0f;
    private const float grabDistanceFromHand = 0.08f;
    private const float grabRadius = 0.1f;

    void Start()
    {
        
    }

    void Update()
    {
        /*----------< RIGHT CONTROLLER >----------*/

        // right trigger
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            //Debug.Log("Right trigger pressed");
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            //Debug.Log("Right trigger released");
        }

        // right grip
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            //Debug.Log("Right grip pressed");
            RaycastHit[] hits = Physics.SphereCastAll(m_RightHand.position - m_RightHand.right * grabDistanceFromHand, grabRadius, -m_RightHand.right, grabDistance);
            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.CompareTag("Throwable") && hit.transform.GetComponent<Rigidbody>())
                    {
                        m_RightGrabbedObject = hit.transform.GetComponent<Rigidbody>();

                        Debug.Log("Grabbed a throwable item with the name: " + hit.transform.name);
                        break;
                    }
                    else if (hit.transform.CompareTag("PlayerGun"))
                    {
                        m_RightGrabbedObject = hit.transform.GetComponent<Rigidbody>();

                        PlayerGun playerGun = hit.transform.GetComponent<PlayerGun>();
                        playerGun.m_IsHeld = true;
                        playerGun.m_Controller = OVRInput.Controller.RTouch;

                        Debug.Log("Grabbed a playergun item with the name: " + hit.transform.name);
                        break;
                    }
                }
            }
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            //Debug.Log("Right grip release");
            if (m_RightGrabbedObject)
            {
                Debug.Log("Dropped a throwable item with the name: " + m_RightGrabbedObject.name);

                if (m_RightGrabbedObject.CompareTag("PlayerGun"))
                {
                    m_RightGrabbedObject.GetComponent<PlayerGun>().m_IsHeld = false;
                }

                m_RightGrabbedObject = null;
            }
        }

        /*----------< LEFT CONTROLLER >----------*/

        // left trigger
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            //Debug.Log("Left trigger pressed");
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            //Debug.Log("Left trigger release");
        }

        // left grip
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            //Debug.Log("Left grip pressed");
            RaycastHit[] hits = Physics.SphereCastAll(m_LeftHand.position + m_LeftHand.right * grabDistanceFromHand, grabRadius, m_LeftHand.right, grabDistance);
            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.CompareTag("Throwable") && hit.transform.GetComponent<Rigidbody>())
                    {
                        m_LeftGrabbedObject = hit.transform.GetComponent<Rigidbody>();

                        Debug.Log("Grabbed a throwable item with the name:" + hit.transform.name);
                        break;
                    }
                    else if (hit.transform.CompareTag("PlayerGun"))
                    {
                        m_LeftGrabbedObject = hit.transform.GetComponent<Rigidbody>();

                        PlayerGun playerGun = hit.transform.GetComponent<PlayerGun>();
                        playerGun.m_IsHeld = true;
                        playerGun.m_Controller = OVRInput.Controller.RTouch;

                        Debug.Log("Grabbed a playergun item with the name: " + hit.transform.name);
                        break;
                    }
                }
            }
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            //Debug.Log("Left grip release");
            if (m_LeftGrabbedObject)
            {
                Debug.Log("Dropped a throwable item with the name: " + m_LeftGrabbedObject.name);

                if (m_LeftGrabbedObject.CompareTag("PlayerGun"))
                {
                    m_LeftGrabbedObject.GetComponent<PlayerGun>().m_IsHeld = false;
                }

                m_LeftGrabbedObject = null;
            }
        }
    }

    private void FixedUpdate()
    {
        AlignObjects();
    }


    /// <summary>
    /// Aligns the left and right objects to the hands positions if they exist
    /// </summary>
    private void AlignObjects()
    {
        /* move the RIGHT grabbed object towards the players hand */
        if (m_RightGrabbedObject)
        {
            /* move the objects rigidbodys position to the hands position */
            m_RightGrabbedObject.velocity = (m_RightHand.position - m_RightGrabbedObject.position) / Time.fixedDeltaTime;

            /* rotate the objects rigidbodys rotation to the players hands rotation */
            m_RightGrabbedObject.maxAngularVelocity = 20;
            Quaternion deltaRot = m_RightHand.rotation * Quaternion.Inverse(m_RightGrabbedObject.transform.rotation);
            Vector3 eulerRot = new Vector3(Mathf.DeltaAngle(0, deltaRot.eulerAngles.x), Mathf.DeltaAngle(0, deltaRot.eulerAngles.y), Mathf.DeltaAngle(0, deltaRot.eulerAngles.z));
            eulerRot *= 0.95f;
            eulerRot *= Mathf.Deg2Rad;
            m_RightGrabbedObject.angularVelocity = eulerRot / Time.fixedDeltaTime;
        }

        /* move the LEFT grabbed object towards the players hand */
        if (m_LeftGrabbedObject)
        {
            /* move the objects rigidbodys position to the hands position */
            m_LeftGrabbedObject.velocity = (m_LeftHand.position - m_LeftGrabbedObject.position) / Time.fixedDeltaTime;

            /* rotate the objects rigidbodys rotation to the players hands rotation */
            m_LeftGrabbedObject.maxAngularVelocity = 20;
            Quaternion deltaRot = m_LeftHand.rotation * Quaternion.Inverse(m_LeftGrabbedObject.transform.rotation);
            Vector3 eulerRot = new Vector3(Mathf.DeltaAngle(0, deltaRot.eulerAngles.x), Mathf.DeltaAngle(0, deltaRot.eulerAngles.y), Mathf.DeltaAngle(0, deltaRot.eulerAngles.z));
            eulerRot *= 0.95f;
            eulerRot *= Mathf.Deg2Rad;
            m_LeftGrabbedObject.angularVelocity = eulerRot / Time.fixedDeltaTime;
        }
    }
}
