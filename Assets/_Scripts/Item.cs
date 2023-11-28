
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory")] 
public class Item : ScriptableObject
{
    public string Name = "Item";
    public Sprite Icon;
    public Item comp1;
    public Item comp2;
}
