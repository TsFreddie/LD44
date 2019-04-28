using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItems : MonoBehaviour
{
    public Buyable[] Buyables;
    public GameObject ShopListItemPrefab;
    public Transform ItemList;
    public GameObject DetailPanel;
    public Text DetailName;
    public Text DetailDesc;
    public Image DetailIcon;
    public Button DetailButton;
    public Text DetailButtonText;
    public Text DetailButtonPrice;
    public AudioClip SoundBuy;
    private int[] levels;
    private int selectedIndex;
    private ShopListItem[] itemPanels;
    private Upgradable[] boughtItems;
    private new AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        levels = new int[Buyables.Length]; 
        boughtItems = new Upgradable[Buyables.Length];
        itemPanels = new ShopListItem[Buyables.Length];
        for (int i = 0; i < Buyables.Length; i++) {
            GameObject item = Instantiate(ShopListItemPrefab);
            item.transform.SetParent(ItemList, false);
            ShopListItem data = item.GetComponent<ShopListItem>();
            data.Icon.sprite = Buyables[i].BuyIcon;
            data.Name.text = Buyables[i].Name;
            data.Price.text = string.Format("x{0}", Buyables[i].Price[0]);
            data.Index = i;
            data.onClick += ShowDetail;
            itemPanels[i] = data;
        }
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Buyables.Length; i++) {
            ShopListItem data = itemPanels[i];
            Buyable item = Buyables[i];
            int buyableLevel = levels[i];
            if (buyableLevel < item.Price.Length) {
                data.Heart.SetActive(true);
                data.Price.text = string.Format("x{0}", item.Price[buyableLevel]);
            } else {
                data.Heart.SetActive(false);
                data.Price.text = "";
            }
        }
    }

    public void StartHealthBlink() {
        if (!DetailButton.interactable) {
            return;
        }

        Buyable item = Buyables[selectedIndex];
        int buyableLevel = levels[selectedIndex];

        if (item.Price.Length <= buyableLevel) {
            HealthDisplay.I.BlinkHearts = 0;
            return;
        }

        if (HealthDisplay.I) {
            HealthDisplay.I.BlinkHearts = item.Price[buyableLevel];
        }
    }

    public void StopHealthBlink() {
        if (HealthDisplay.I) {
            HealthDisplay.I.BlinkHearts = 0;
        }
    }

    public void ShowDetail(int index) {
        selectedIndex = index;
        int buyableLevel = levels[selectedIndex];
        Buyable item = Buyables[selectedIndex];
        DetailPanel.SetActive(true);
        // Price
        if (item.Price.Length <= buyableLevel) {
            DetailButton.gameObject.SetActive(false);
        } else {
            DetailButton.gameObject.SetActive(true);
            DetailButton.interactable = Character.I.SpendableHealth >= item.Price[buyableLevel];
        }

        if (buyableLevel == 0) {
            DetailButtonText.text = "Buy";
        } else {
            DetailButtonText.text = "Upgrade";
        }
        

        // Data
        DetailName.text = item.Name;
        string descText = item.Description;
        if (buyableLevel < item.LevelDescription.Length) {
            descText += "\n\n" + item.LevelDescription[buyableLevel];
        }
        DetailDesc.text = descText;
        DetailIcon.sprite = item.BuyIcon;
        if (item.Price.Length > buyableLevel) {
            DetailButtonPrice.text = string.Format("x{0}", item.Price[buyableLevel]);
        }
    }

    public void Buy() {
        int buyableLevel = levels[selectedIndex];
        Buyable item = Buyables[selectedIndex];

        if (item.Price.Length <= buyableLevel) {
            return;
        }
        levels[selectedIndex]++;
        int price = item.Price[buyableLevel];
        if (buyableLevel == 0) {
            if (item.BuyableType == BuyableType.Weapon) {
                boughtItems[selectedIndex] = Character.I.GiveWeapon(item.Prefab);
            } else {
                boughtItems[selectedIndex] = Character.I.GivePerk(item.Prefab);
            }
        } else if (boughtItems[selectedIndex] != null) {
            boughtItems[selectedIndex].UpgradeTo(buyableLevel);
        }
        HealthDisplay.I.BlinkHearts = 0;
        ShowDetail(selectedIndex);
        if (audio != null && SoundBuy != null) {
            audio.PlayOneShot(SoundBuy);
        }
    }
}
