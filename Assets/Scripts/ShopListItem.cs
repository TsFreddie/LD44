using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopListItem : MonoBehaviour
{
    public Color CanBuyColor;
    public Color CannotBuyColor;
    public Text Name;
    public Text Price;
    public Image Icon;
    public Button Button;
    public GameObject Heart;
    public int Index = -1;
    
    public delegate void ListItemButtonClicked(int index);
    public event ListItemButtonClicked onClick;

    public bool CanBuy {
        set {
            if (value) {
                ColorBlock cb = Button.colors;
                cb.normalColor = CanBuyColor;
                Button.colors = cb;
            } else {
                ColorBlock cb = Button.colors;
                cb.normalColor = CannotBuyColor;
                Button.colors = cb;
            }
        }
    }

    public void Click() {
        onClick(Index);
    }
}
