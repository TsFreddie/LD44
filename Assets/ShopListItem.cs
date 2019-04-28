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
    public int Index = -1;

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
}
