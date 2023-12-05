using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : Health
{
    [SerializeField] private Image healthBar;
    [SerializeField] private float maxStamina;
    private float currentStamina;
    [SerializeField] private Image staminaBar;
    [SerializeField] private float timeToRecoverStamina;
    private float currentTimeToRecoverStamina;


    private void Start()
    {
        healthBar = healthBar.GetComponent<Image>();
        staminaBar = staminaBar.GetComponent<Image>();
        currentStamina = maxStamina;
        currentHealth = maxHealth;
    }

    public bool TakeStamina(float stamina)
    {
        if (currentStamina > stamina)
        {
            currentStamina -= stamina;
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (currentStamina < maxStamina)
        {
            if (currentTimeToRecoverStamina <= 0)
            {
                currentStamina += Time.deltaTime * 5f;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
        }
        RedrawBars();
    }

    private void RedrawBars()
    {
        staminaBar.fillAmount = currentStamina / maxStamina;
        healthBar.fillAmount = currentHealth / maxHealth;
    }

}
