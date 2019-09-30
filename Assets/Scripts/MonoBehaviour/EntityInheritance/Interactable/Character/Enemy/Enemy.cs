using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{

    [Space(10),Header("Enemy settings")]
    [SerializeField] EnemyStateMachine[] enemyStateMachines;
    [SerializeField] int m_startStateMachine;
    [SerializeField] float m_lookSpeed = 20;
    [SerializeField] bool m_startBehaviourOnStart = true;
    [SerializeField] Ragdoll m_ragdoll;
    [SerializeField] NavMeshAgent m_agent;
    [SerializeField] Collider m_collider;

    [SerializeField] Weapon m_weapon;
    [SerializeField] EnemyEar m_ear;

    public Weapon Weapon { get => m_weapon; private set => m_weapon = value; }
    public EnemyEar Ear { get => m_ear; private set => m_ear = value; }

    [Space(10), Header("Enemy behaviour")]
    [SerializeField] EnemyStateMachine m_stateWhenDamaged;
    [SerializeField] EnemyStateMachine m_stateWhenAlerted;
    [SerializeField] EnemyStateMachine m_stateWhenHearShoot;

    public override void Start()
    {
        base.Start();

        if(m_startBehaviourOnStart) StartBehaviour();

        if (Ear != null)
        {
            Ear.OnShotHeard += HearNoise;
        }

    }

    private void StartBehaviour()
    {
        enemyStateMachines[m_startStateMachine].OnStateStart();
    }

    public override void TakeDamage(DamageInfo p_damageInfo)
    {
        base.TakeDamage(p_damageInfo);

        if (m_stateWhenDamaged != null)
        {
            GetActiveState()?.OnStateCancel();
            m_stateWhenDamaged.OnStateStart();
        }
    }

    public override void Death()
    {
        base.Death();

        for (int i = 0; i < enemyStateMachines.Length; i++)
        {
            enemyStateMachines[i].StateActive = false;
            enemyStateMachines[i].enabled = false;
        }
        m_agent.isStopped = true;
        m_ear.enabled = false;
        m_collider.enabled = false;
        m_ragdoll.SetKinematic(false);
    }

    public void Look(Vector3 p_target)
    {
        //transform.LookAt(p_targetDir, transform.up);
        Vector3 dir = CustomMethod.GetDirection(transform.position, p_target);
        SmoothLookAt(dir);
    }

    void HearNoise(Vector3 p_noisePos)
    {
        if(m_stateWhenHearShoot != null)
        {
            GetActiveState()?.OnStateCancel();
            m_stateWhenHearShoot.OnStateStart();
        }
    }

    void SmoothLookAt(Vector3 newDirection)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newDirection), Time.deltaTime * m_lookSpeed);
    }

    EnemyStateMachine GetActiveState()
    {
        for (int i = 0; i < enemyStateMachines.Length; i++)
        {
            if (enemyStateMachines[i].StateActive)
            {
                return enemyStateMachines[i];
            }
        }

        return null;
    }
}
