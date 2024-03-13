using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FTLRepairDrone : MonoBehaviour
{
    public BasicPart target;
    public BasicPart dock;
    public bool isFixing = false;
    public float speed;
    public int coolDown;

    public IEnumerator RepairCycle(BasicPart p)
    {
        target = p;
        yield return new WaitForSeconds(coolDown);
        p.HP = p.MaxHP;
        p.MaxEnergy = p.ReservedEnergy;
        isFixing = false;
        target = null;
        yield return null;
        //StopAllCoroutines();
    }

    private void Update()
    {
        if (isFixing && target != null)
        {
            transform.position = Vector2.Lerp(transform.position, target.transform.position,Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, dock.transform.position, Time.deltaTime * speed/2f);
        }
    }
}
