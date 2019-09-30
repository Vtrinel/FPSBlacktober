using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine_Research : EnemyStateMachine
{
    [SerializeField] float visualTime = 10;
    [SerializeField] float noiseTime = 20;

    [SerializeField] EnemyStateMachine m_stateWhenPlayerSpotted;

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (_enemyFieldOfView.HasTargetOnSight)
        {
            OnStateEnd(m_stateWhenPlayerSpotted);
            return;
        }

        float timeSinceLastNoise = Time.time - _enemy.Ear.LastSoundTime;
        float timeSinceLastVisual = Time.time - _enemyFieldOfView.LastVisualTime;

        if(timeSinceLastVisual < visualTime && _enemyFieldOfView.HasAlreadySawTarget)
        {
            SetDestination(_enemyFieldOfView.TargetLocation);
            return;
        }

        if(timeSinceLastNoise < noiseTime)
        {
            SetDestination(_enemy.Ear.HeardPosition);
            return;
        }

        OnStateEnd();
    }

    void SetDestination(Vector3 p_targetPos)
    {
        _navMeshAgent.destination = p_targetPos;
    }
}
