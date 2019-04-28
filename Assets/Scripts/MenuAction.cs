using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAction : MonoBehaviour
{
    public GameObject Shop;
    public GameObject ShopNotice;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Open Shop")) {
            ToggleShop();
        }
    }

    public void ToggleShop() {
        Shop.SetActive(!Shop.activeSelf);
        ShopNotice.SetActive(!Shop.activeSelf);
        if (Shop.activeSelf) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }
}
