using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Entity : MonoBehaviour
{
    [Header("Entity settings")]
    [SerializeField,ReadOnly] protected GameManager _gameManager;
    [SerializeField] protected float _timeBeforeDestroy = 0.5f;
    [SerializeField] protected Renderer[] _renderers;
    [SerializeField] protected Collider[] _colliders;

    public virtual void Awake()
    {
        _gameManager = GameManager.instance;
    }

    public virtual void OnEnable()
    {
        _gameManager.RegisterEntity(this);
    }

    public virtual void OnDisable()
    {
        _gameManager.RemoveEntity(this);
    }

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        
    }

    public virtual void LateUpdate()
    {

    }

    public virtual void DestroyEntity()
    {
        StartCoroutine(DestroyingEntity());
    }

    protected IEnumerator DestroyingEntity()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].enabled = false;
        }

        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = false;
        }

        yield return new WaitForSeconds(_timeBeforeDestroy);
        gameObject.SetActive(false);
    }

    public virtual void PlayerGetAmmo(int p_ammo)
    {

    }

    public virtual void PlayerGetGrenade(int p_grenade)
    {

    }

    public virtual void PlayerGetHealth(int p_healthPoint)
    {

    }

    public virtual void OnPlayerShoot(PlayerShootInfo p_playerShootInfo)
    {

    }

    public virtual void OnGameOver()
    {

    }

}
