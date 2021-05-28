using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    List<InputDevice> m_InputDevices = new List<InputDevice>();

    public Material m_Material;
    public Canvas m_Canvas;

    public GameObject m_Hand;
    public Text[] m_Texts;

    // Start is called before the first frame update
    void Start()
    {
        InputDevices.GetDevices(m_InputDevices);

        if (m_InputDevices[0] != null)
            if(m_InputDevices[0].name == "Oculus Go")
                m_Material.color = Color.red;

        for(int i = 0; i < m_InputDevices.Count; i++)
        {
            //m_Texts[i].transform.position = new Vector3(0.0f, 10.0f * i);
            m_Texts[i].text = m_InputDevices[i].name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //List<InputFeatureUsage> features = new List<InputFeatureUsage>();
        //InputDevice device = m_InputDevices[1];
        //
        //device.TryGetFeatureUsages(features);
        //
        //foreach(var feature in features)
        //{
        //    m_Hand.transform.rotation = device.TryGetFeatureValue()
        //}
    }
}
