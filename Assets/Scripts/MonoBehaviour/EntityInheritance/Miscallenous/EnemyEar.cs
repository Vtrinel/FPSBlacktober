using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class EnemyEar : Entity
{
    [SerializeField] float m_listenRadius = 10;
    [SerializeField,ReadOnly] bool hasPosition;
    [SerializeField,ReadOnly] Vector3 heardPosition;
    [SerializeField,ReadOnly] float lastSoundTime;

    public event Action<Vector3> OnShotHeard = delegate { };

    public Vector3 HeardPosition { get => heardPosition; private set => heardPosition = value; }
    public bool HasPosition { get => hasPosition; private set => hasPosition = value; }
    public float LastSoundTime { get => lastSoundTime; private set => lastSoundTime = value; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, m_listenRadius);
    }

    public override void OnPlayerShoot(PlayerShootInfo p_playerShootInfo)
    {
        base.OnPlayerShoot(p_playerShootInfo);

        if (Vector3.Distance(transform.position, p_playerShootInfo.ShootPosition) > m_listenRadius) return;

        HeardPosition = p_playerShootInfo.ShootPosition;
        LastSoundTime = p_playerShootInfo.ShootTime;
        HasPosition = true;
        HeardShot(p_playerShootInfo);
    }

    public void HeardShot(PlayerShootInfo p_info)
    {
        OnShotHeard(p_info.ShootPosition);
    }
}
