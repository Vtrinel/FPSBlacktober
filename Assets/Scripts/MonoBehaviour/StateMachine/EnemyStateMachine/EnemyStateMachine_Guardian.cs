using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine_Guardian : EnemyStateMachine
{
    Vector3 m_iniPos;
    Vector3 m_iniRot;

    private void Start()
    {
        m_iniPos = _enemy.transform.position;
        m_iniRot = _enemy.transform.eulerAngles;
    }

    public override void OnStateStart()
    {
        base.OnStateStart();
    }

    public override void OnStateEnd()
    {
        _navMeshAgent.isStopped = false;
        base.OnStateEnd();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        //!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.1f && _enemyFieldOfView.HasTargetOnSight)
        if (CustomMethod.AlmostEqual(_enemy.transform.position,m_iniPos,0.2f))
        {
            Debug.Log("On destination", this);
            //_navMeshAgent.isStopped = true;
            _enemy.transform.position = m_iniPos;
            _enemy.transform.eulerAngles = m_iniRot;
        }
        else
        {
            _navMeshAgent.destination = m_iniPos;

        }

        if (_enemyFieldOfView.HasTargetOnSight)
        {
            OnStateEnd();
        }
    }
}
