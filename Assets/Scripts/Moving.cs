using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    private Vector3 m_originalPosition;
    private float m_time = 0;
    [SerializeField] private float m_magnitude;
    [SerializeField] private float timeDelay;

    private void Start()
    {
        m_originalPosition = transform.parent.transform.position;
    }
    private void FixedUpdate()
    {
        if (m_time < timeDelay)
        {
            m_time += Time.fixedDeltaTime;
        }
        else
        {
            transform.parent.transform.position = m_originalPosition + transform.rotation * (m_magnitude * Mathf.Sin(Time.time - timeDelay) * transform.parent.transform.right);
        }
    }   

    private void OnTriggerEnter(Collider other)
    {   
        if (other.CompareTag("Player"))
        {
            other.transform.parent.parent.parent = this.transform.parent;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent.parent.parent = null;
        }
    }
}

