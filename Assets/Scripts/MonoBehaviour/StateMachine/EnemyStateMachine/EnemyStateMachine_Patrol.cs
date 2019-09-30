using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using NaughtyAttributes;

/// <summary>
/// In this state the AI moves endlessly towards all the points of a PatrolPath
/// </summary>
public class EnemyStateMachine_Patrol : EnemyStateMachine
{
    [Header("Patrol settings")]
    [SerializeField,Required] PatrolPath m_patrolPath;
    [SerializeField] int m_currentPatrolPoint;
    [SerializeField] UnityEvent m_onPatrolPointReached;

    bool firstStart;

    public override void OnStateStart()
    {
        base.OnStateStart();
        _enemyFieldOfView.OnTargetFound += OnStateEnd;
        SetDestination();
    }

    public override void OnStateEnd()
    {
        _enemyFieldOfView.OnTargetFound -= OnStateEnd;
        base.OnStateEnd();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
    }

    private void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (m_patrolPath.PatrolPoints.Count == 0) return;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        m_currentPatrolPoint = (m_currentPatrolPoint + 1) % m_patrolPath.PatrolPoints.Count;

        m_onPatrolPointReached.Invoke();
        SetDestination();
    }

    private void SetDestination()
    {
        // Set the agent to go to the currently selected destination.
        _navMeshAgent.destination = m_patrolPath.PatrolPoints[m_currentPatrolPoint].PatrolPointPosition;
    }
}
