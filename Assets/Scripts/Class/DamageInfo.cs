using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
    [SerializeField] float m_damagePoints;
    [SerializeField] Vector3 m_damageContactPosition;
    [SerializeField] Vector3 m_damageContactNormal;

    public DamageInfo(float p_damagePoint,Vector3 p_contactPosition, Vector3 p_contactNormal)
    {
        DamagePoints = p_damagePoint;
        DamageContactPosition = p_contactPosition;
        DamageContactNormal = p_contactNormal;
    }

    public float DamagePoints { get => m_damagePoints; private set => m_damagePoints = value; }
    public Vector3 DamageContactPosition { get => m_damageContactPosition; private set => m_damageContactPosition = value; }
    public Vector3 DamageContactNormal { get => m_damageContactNormal; private set => m_damageContactNormal = value; }
}
