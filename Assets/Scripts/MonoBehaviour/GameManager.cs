using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] List<Entity> m_entities;

    [Space]

    [SerializeField] float m_timeBeforeReload = 5;

    public List<Entity> Entities { get => m_entities; private set => m_entities = value; }

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        Cursor.visible = false;
    }

    public void RegisterEntity(Entity p_entity)
    {
        Entities.Add(p_entity);
    }

    public void RemoveEntity(Entity p_entity)
    {
        if (Entities.Contains(p_entity))
        {
            Entities.Remove(p_entity);
        }
    }

    //ENTITY CALLBACK

    public void CallOnPlayerGetAmmo(int p_ammo)
    {
        for (int i = 0; i < Entities.Count; i++)
        {
            Entities[i].PlayerGetAmmo(p_ammo);
        }
    }

    public void CallOnPlayerGetGrenade(int p_grd)
    {
        for (int i = 0; i < Entities.Count; i++)
        {
            Entities[i].PlayerGetGrenade(p_grd);
        }
    }

    public void CallOnPlayerGetHealth(int p_hp)
    {
        for (int i = 0; i < Entities.Count; i++)
        {
            Entities[i].PlayerGetHealth(p_hp);
        }
    }

    public void CallOnPlayerShoot(PlayerShootInfo p_shootInfo)
    {
        for (int i = 0; i < Entities.Count; i++)
        {
            Entities[i].OnPlayerShoot(p_shootInfo);
        }
    }

    public void CallOnGameOver()
    {
        for (int i = 0; i < Entities.Count; i++)
        {
            Entities[i].OnGameOver();
        }

        StartCoroutine(ReloadScene());
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(m_timeBeforeReload);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
