
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory")] 
public class Item : ScriptableObject
{
    public string Name = "Item";
    public Sprite Icon;
}
