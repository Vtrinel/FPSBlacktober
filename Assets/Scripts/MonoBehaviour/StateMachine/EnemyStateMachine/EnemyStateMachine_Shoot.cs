using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In this state, the AI must shoot at the last known position of the player
/// </summary>
public class EnemyStateMachine_Shoot : EnemyStateMachine
{
    [SerializeField] float m_blindShotDuration = 10;

    Vector3 m_targetPos;
    IEnumerator count;

    public override void OnStateStart()
    {
        base.OnStateStart();
        _enemyFieldOfView.OnTargetFound += TargetFound;
        _enemyFieldOfView.OnTargetLost += TargetLost;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (_enemyFieldOfView.HasTargetOnSight)
        {
            m_targetPos = _enemyFieldOfView.TargetLocation;
        }

        _enemy.Look(m_targetPos);
        Debug.Log("SHOOT", this);
        _enemy.Weapon.ShootOnTarget(m_targetPos);
    }

    public override void OnStateEnd()
    {
        base.OnStateEnd();
        _enemyFieldOfView.OnTargetFound -= TargetFound;
        _enemyFieldOfView.OnTargetLost -= TargetLost;
    }

    void TargetLost()
    {
        m_targetPos = _enemyFieldOfView.TargetLocation;
        count = BlindShoot();
        StartCoroutine(count);
        Debug.Log("Lost Target");
    }

    void TargetFound()
    {
        if(count != null)
        {
            StopCoroutine(count);
            count = null;
        }
    }

    IEnumerator BlindShoot()
    {
        yield return new WaitForSeconds(m_blindShotDuration);
        OnStateEnd();
    }


}
