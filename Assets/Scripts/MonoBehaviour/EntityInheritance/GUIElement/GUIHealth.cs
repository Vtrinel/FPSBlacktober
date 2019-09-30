using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHealth : GUIElement
{
    [SerializeField] Image m_healthBar;

    Player player;
    private float maxHealth;
    private float currentHealth;

    public override void OnEnable()
    {
        base.OnEnable();
        player = Player.instance;
        maxHealth = player.Health.LifePoint;
        currentHealth = player.Health.LifePoint;
    }

    public override void Update()
    {
        base.Update();
        FillBar();
    }

    void FillBar()
    {
        currentHealth = player.Health.LifePoint;
        m_healthBar.fillAmount = currentHealth / maxHealth;
    }
}
