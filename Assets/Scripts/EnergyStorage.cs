using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyStorage : MonoBehaviour
{
    public int MaxPoints;
    public int FreePoints;
    public GameObject[] powerVisualiser;
    
    public void POWER(BasicPart part)
    {
        if (part.MaxEnergy != 0 && part.UsingEnergy < part.MaxEnergy && FreePoints > 0)
        {
            part.UsingEnergy++;
            foreach(GameObject g in part.powers)
            {
                if (!g.active)
                {
                    g.SetActive(true);
                    break;
                }
            }
            FreePoints--;
            for (int i = powerVisualiser.Length - 1; i >= 0; i--)
            {
                if (powerVisualiser[i].active)
                {
                    powerVisualiser[i].SetActive(false);
                    break;
                }
            }
        }
    }

    public void POWERWeapon(BasicWeapon weapon)
    {
        if (weapon.MaxEnergy != 0 && weapon.UsingEnergy < weapon.MaxEnergy && FreePoints > 0)
        {
            weapon.UsingEnergy++;
            foreach (GameObject g in weapon.powers)
            {
                if (!g.active)
                {
                    g.SetActive(true);
                    break;
                }
            }
            FreePoints--;
            for (int i = powerVisualiser.Length - 1; i >= 0; i--)
            {
                if (powerVisualiser[i].active)
                {
                    powerVisualiser[i].SetActive(false);
                    break;
                }
            }
        }
    }
    public void UnpowerWeapon(BasicWeapon weapon)
    {
        if (weapon.UsingEnergy > 0)
        {
            weapon.UsingEnergy--;
            foreach (GameObject g in powerVisualiser)
            {
                if (!g.active)
                {
                    g.SetActive(true);
                    break;
                }
            }
            FreePoints++;
            for (int i = weapon.powers.Length - 1; i >= 0; i--)
            {
                if (weapon.powers[i].active)
                {
                    weapon.powers[i].SetActive(false);
                    break;
                }
            }
        }
    }
    public void Unpower(BasicPart part)
    {
        if (part.UsingEnergy > 0)
        {
            part.UsingEnergy--;
            foreach (GameObject g in powerVisualiser)
            {
                if (!g.active)
                {
                    g.SetActive(true);
                    break;
                }
            }
            FreePoints++;
            for (int i = part.powers.Length - 1; i >= 0; i--)
            {
                if (part.powers[i].active)
                {
                    part.powers[i].SetActive(false);
                    break;
                }
            }
        }
    }
}
