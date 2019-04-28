using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItems : MonoBehaviour
{
    public Buyable[] Buyables;
    public GameObject ShopListItemPrefab;
    public Transform ItemList;
    private int[] levels;
    // Start is called before the first frame update
    void Start()
    {
        levels = new int[Buyables.Length]; 
        for (int i = 0; i < Buyables.Length; i++) {
            GameObject item = Instantiate(ShopListItemPrefab);
            item.transform.SetParent(ItemList, false);
            ShopListItem data = item.GetComponent<ShopListItem>();
            data.Icon.sprite = Buyables[i].BuyIcon;
            data.Name.text = Buyables[i].Name;
            data.Price.text = string.Format("x{0}", Buyables[i].Price[levels[i]]);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
