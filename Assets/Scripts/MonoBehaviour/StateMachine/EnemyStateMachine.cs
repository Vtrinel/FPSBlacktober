using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class EnemyStateMachine : MonoBehaviour
{
    [Header("State Machine References")]
    [SerializeField,Required] protected FieldOfView _enemyFieldOfView;
    [SerializeField,Required] protected NavMeshAgent _navMeshAgent;
    [SerializeField,Required] protected Enemy _enemy;

    [Header("State Machine values")]
    [SerializeField] bool m_stateActive;

    [Header("State Machine Behaviour")]
    [SerializeField] protected EnemyStateMachine _OnEndStateMachine;

    public bool StateActive { get => m_stateActive; set => m_stateActive = value; }

    [Button]
    protected void GetReferences()
    {
        _enemyFieldOfView = GetComponent<FieldOfView>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _enemy = GetComponent<Enemy>();

        if(_enemyFieldOfView == null)
        {
            _enemyFieldOfView = GetComponentInParent<FieldOfView>();
            _navMeshAgent = GetComponentInParent<NavMeshAgent>();
            _enemy = GetComponentInParent<Enemy>();
        }
    }

    public void Update()
    {
        if (!StateActive) return;
        StateUpdate();
    }

    public virtual void OnStateStart()
    {
        StateActive = true;
    }

    public virtual void StateUpdate()
    {
        if (!StateActive) return;
    }

    public virtual void OnStateEnd(EnemyStateMachine p_nextState)
    {
        StateActive = false;
        p_nextState.OnStateStart();
    }

    public virtual void OnStateEnd()
    {
        StateActive = false;
        _OnEndStateMachine.OnStateStart();
    }

    public virtual void OnStateCancel()
    {
        StateActive = false;
    }
}
