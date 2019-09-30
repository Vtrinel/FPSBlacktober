using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine_Chase : EnemyStateMachine
{
    [Header("Chase state settings")]
    [SerializeField] float m_distanceToEndState = 0.25f;
    [SerializeField] EnemyStateMachine m_stateWhenReachLastKnownPosition;
    [SerializeField] EnemyStateMachine m_stateWhenReachTarget;

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (_enemyFieldOfView.HasTargetOnSight)
        {
            _navMeshAgent.destination = _enemyFieldOfView.TargetLocation;
        }

        //Reach destination
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < m_distanceToEndState)
        {
            if (_enemyFieldOfView.HasTargetOnSight)
            {
                OnStateEnd(m_stateWhenReachTarget);
            }
            else
            {
                OnStateEnd(m_stateWhenReachLastKnownPosition);
            }
        }
    }

    void OnTargetLost()
    {
        _navMeshAgent.destination = _enemyFieldOfView.TargetLocation;
    }


    public override void OnStateStart()
    {
        base.OnStateStart();
        _enemyFieldOfView.OnTargetLost += OnTargetLost;
    }

    public override void OnStateEnd(EnemyStateMachine p_nextState)
    {
        _enemyFieldOfView.OnTargetLost -= OnTargetLost;
        base.OnStateEnd(p_nextState);
    }
}
