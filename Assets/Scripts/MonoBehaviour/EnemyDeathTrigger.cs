using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDeathTrigger : MonoBehaviour
{
    [SerializeField] List<Enemy> enemies;
    [SerializeField] UnityEvent deathEvent;

    private void Update()
    {
        int l_count = 0;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].isDead) l_count++;
        }

        if(l_count == enemies.Count)
        {
            deathEvent.Invoke();
            this.enabled = false;
        }
    }
}
