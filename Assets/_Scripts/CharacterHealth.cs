using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : Health
{
    [SerializeField] private Image healthBar;
    [SerializeField] private float maxStamina;
    [SerializeField] private float maxAdrenaline;
    public float currentStamina;
    public float oxygenResistance;
    [SerializeField] private Image staminaBar;
    [SerializeField] private float timeToRecoverStamina;
    private float currentTimeToRecoverStamina;

    [SerializeField] private Image adrenalineBar;
    [SerializeField] private float timeToRecoverAdrenaline;
    [SerializeField] private float currentAdrenaline;
    [SerializeField] private GameObject canUseAdrText;
    public float baseAdr;
    public Transform spawnPos;

    public bool isUsingAdr;
    public bool isDamageByAdr;

    private bool bool_AdrenalineByUsingChange = false;

    private void Start()
    {

        healthBar = healthBar.GetComponent<Image>();
        staminaBar = staminaBar.GetComponent<Image>();
        currentStamina = maxStamina;
        currentHealth = maxHealth;
    }

    private float currentTime_AdrenalineByUsingChange;
    private void AdrenalineByUsingChange()
    {
        currentTime_AdrenalineByUsingChange -= Time.deltaTime;
        while (isUsingAdr && currentTime_AdrenalineByUsingChange <= 0)
        {
            if (currentAdrenaline <= 0)
            {
                isUsingAdr = false;
            }

            if (isUsingAdr)
            {
                currentAdrenaline -= 2f;
            }

            print("AdrenalineByUsingChange");
            currentTime_AdrenalineByUsingChange = 1f;
            break;
        }
    }

    private IEnumerator AdrenalineByHP()
    {
        while (!isUsingAdr)
        {
            TakeDamage(2f);
            yield return new WaitForSeconds(1f);
            print("AdrenalineByHP");
        }
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
    
    public void TakeAdrenaline(float adr)
    {
        currentAdrenaline += adr;
        currentAdrenaline = Mathf.Clamp(currentAdrenaline, 0, maxAdrenaline);
    }

    private void Update()
    {
        if(bool_AdrenalineByUsingChange) AdrenalineByUsingChange();
        
        if (isDead)
        {
            Respawn();
            
        }

        if (Input.GetKeyDown(KeyCode.L)) Time.timeScale = 10f;
        if (Input.GetKeyDown(KeyCode.B)) Time.timeScale = 1f;
        
        if (currentAdrenaline >= 100)
        {
            
            canUseAdrText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.V))
            {
                canUseAdrText.SetActive(false);
                isUsingAdr = true;
                bool_AdrenalineByUsingChange = true;
                StopAllCoroutines();
            }
        }

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

    private void Respawn()
    {
        transform.position = spawnPos.position;
        isDead = false;
        currentAdrenaline = 0;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }
    
    
    private void RedrawBars()
    {
        staminaBar.fillAmount = currentStamina / maxStamina;
        healthBar.fillAmount = currentHealth / maxHealth;
        adrenalineBar.fillAmount = currentAdrenaline / maxAdrenaline;
    }

}
