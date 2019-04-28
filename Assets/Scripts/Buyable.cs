using UnityEngine;

public enum BuyableType {
    Weapon,
    Perk,
}

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Buyable", order = 1)]
public class Buyable : ScriptableObject {
    public string Name;
    public string Description;
    public string[] LevelDescription;
    public string[] ActionKeyNames;
    public Sprite BuyIcon;
    public int[] Price;
    public int MaxLevel {
        get {
            return Price.Length;
        }
    }
    public BuyableType BuyableType;
    public GameObject Prefab;
}