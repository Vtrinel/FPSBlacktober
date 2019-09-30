using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class EventTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent m_onTriggerEnterEvent;
    [SerializeField] UnityEvent m_onTriggerExitEvent;
    BoxCollider m_collider;

    [Space]
    [SerializeField] Color m_colliderColor = Color.green;
    [SerializeField,Range(0.0f,1.0f)] float m_alpha = 0.25f;

    private void OnDrawGizmos()
    {
        m_collider = GetComponent<BoxCollider>();
        Gizmos.color = CustomMethod.GetColorWithAlpha(m_colliderColor, m_alpha);
        Gizmos.DrawCube(transform.position + m_collider.center, m_collider.size);
    }

    private void Start()
    {
        if(!m_collider) m_collider = GetComponent<BoxCollider>();
        m_collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        IPlayer player = other.GetComponent<IPlayer>();

        if (player != null)
        {
            m_onTriggerEnterEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IPlayer player = other.GetComponent<IPlayer>();

        if (player != null)
        {
            m_onTriggerExitEvent.Invoke();
        }
    }
}
