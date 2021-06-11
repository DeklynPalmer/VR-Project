using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownScale : MonoBehaviour
{
    [Header("Wait Time:")]
    [Tooltip("In Seconds.")]
    public float m_WaitTime = 2.0f;
    private float m_WaitTimeElapsed = 0.0f;

    [Header("Scale Down Time:")]
    [Tooltip("In Seconds.")]
    public float m_ScaleTime = 1.0f;
    private float m_ScaleValue;

    void Update()
    {
        /* If we are still in the wait time. */
        if (m_WaitTimeElapsed < m_WaitTime)
            m_WaitTimeElapsed += Time.deltaTime;

        /* If we are no longer waiting. */
        else
        {
            /* Not going to be acurrate, but meh. */
            m_ScaleValue = (Time.deltaTime / m_ScaleTime);

            /* Decrease the Scale Value. */
            gameObject.transform.localScale -= new Vector3(m_ScaleValue, m_ScaleValue, m_ScaleValue);
        }

        /* Check if we have shrunk to zero. */
        if (gameObject.transform.localScale.x <= 0)
            Destroy(gameObject);
    }
}
