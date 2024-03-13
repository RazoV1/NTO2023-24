using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPart : MonoBehaviour
{
    public int HP;
    public int MaxHP;
    public int ReservedEnergy;
    public int RepairSpeed = 1;
    public int MaxEnergy;
    public int UsingEnergy;
    public GameObject[] powers;

    private void Start()
    {
        ReservedEnergy = MaxEnergy;
    }
}
