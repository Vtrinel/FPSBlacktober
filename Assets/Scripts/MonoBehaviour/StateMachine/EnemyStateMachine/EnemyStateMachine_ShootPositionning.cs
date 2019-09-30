using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine_ShootPositionning : EnemyStateMachine
{
    [SerializeField] float m_shootDistance = 5;
    [SerializeField] float m_lostDistance = 2;

    [SerializeField] EnemyStateMachine m_stateWhenCanShoot;
    [SerializeField] EnemyStateMachine m_stateWhenLostTarget;

    private void Start()
    {
        if(m_shootDistance > _enemyFieldOfView.viewRadius)
        {
            Debug.LogWarning("shootDistance seems too high");
        }
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if(_enemyFieldOfView.HasTargetOnSight) _navMeshAgent.destination = _enemyFieldOfView.TargetLocation;

        //Reach destination + see player
        if (_navMeshAgent.remainingDistance < m_shootDistance && _enemyFieldOfView.HasTargetOnSight)
        {
            OnStateEnd(m_stateWhenCanShoot);
        }

        //Lost player
        if (_navMeshAgent.remainingDistance < m_lostDistance && !_enemyFieldOfView.HasTargetOnSight)
        {
            OnStateEnd(m_stateWhenLostTarget);
        }
    }

    void HearNoise(Vector3 p_noisePos)
    {
        if (!_enemyFieldOfView.HasTargetOnSight)
        {
            _navMeshAgent.destination = p_noisePos;
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
        _enemy.Ear.OnShotHeard += HearNoise;
        _navMeshAgent.destination = _enemyFieldOfView.TargetLocation;
    }

    public override void OnStateEnd(EnemyStateMachine p_nextState)
    {
        _enemyFieldOfView.OnTargetLost -= OnTargetLost;
        _enemy.Ear.OnShotHeard -= HearNoise;
        _navMeshAgent.destination = transform.position;
        base.OnStateEnd(p_nextState);
    }

    public override void OnStateCancel()
    {
        _enemyFieldOfView.OnTargetLost -= OnTargetLost;
        _enemy.Ear.OnShotHeard -= HearNoise;
        base.OnStateCancel();
    }
}
