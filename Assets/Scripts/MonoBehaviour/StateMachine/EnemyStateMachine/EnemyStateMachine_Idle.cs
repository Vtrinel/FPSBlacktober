using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine_Idle : EnemyStateMachine
{
    [Header("Idle settings")]
    [SerializeField] float m_idleDuration = 2;

    public override void OnStateStart()
    {
        base.OnStateStart();
        _navMeshAgent.isStopped = true;
        StartCoroutine(StateDuration());
    }

    public override void OnStateEnd()
    {
        base.OnStateEnd();
        _navMeshAgent.isStopped = false;
    }

    IEnumerator StateDuration()
    {

        yield return new WaitForSeconds(m_idleDuration);
        OnStateEnd();
    }
}
