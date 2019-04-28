using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : SingletonMonoBehaviour<HealthDisplay>
{
    public GameObject LinePrefab;
    public GameObject HeartPrefab;
    public GameObject LockedHeartPrefab;
    public int Max = 0;
    public int Value = 0;
    public int HeartsPerLine = 10;
    public int BlinkHearts = 0;
    private int blinking = 0;
    private float blinkAlpha = 1;
    private List<Transform> Lines;
    private List<Image> Hearts;
    private int filled = 0;
    
    protected override void SingletonAwake() {
        Lines = new List<Transform>();
        Hearts = new List<Image>();
    }
    void Start()
    {
        filled = 0;
    }

    void Update()
    {
        if (Max < 0) {
            Max = 0;
        }

        int targetLines = Mathf.CeilToInt(Max / (float)HeartsPerLine);
        while (targetLines > Lines.Count) {
            GameObject newLine = Instantiate(LinePrefab);
            newLine.transform.SetParent(transform);
            newLine.transform.localScale = Vector3.one;
            Lines.Add(newLine.transform);
        }

        while (Max > Hearts.Count) {
            int line = Hearts.Count / HeartsPerLine;
            GameObject newHeart;
            if (Hearts.Count < 5) {
                newHeart = Instantiate(LockedHeartPrefab);
            } else {
                newHeart = Instantiate(HeartPrefab);
            }
                
            Image image = newHeart.GetComponent<Image>();
            if (image == null) {
                Destroy(newHeart);
                return;
            }
            newHeart.transform.SetParent(Lines[line]);
            newHeart.transform.localScale = Vector3.one;
            Hearts.Add(image);
        }

        while (Max < Hearts.Count) {
            Image image = Hearts[Hearts.Count - 1];
            Hearts.RemoveAt(Hearts.Count - 1);
            Destroy(image.gameObject);

        }

        if (Value > Max) {
            Value = Max;
        }

        if (Value < 0) {
            Value = 0;
        }
        
        while (filled < Value) {
            if (filled < Hearts.Count) {
                Hearts[filled].fillAmount = 1;
            }
            filled++;
        }

        while (filled > Value) {
            filled--;
            if (filled < Hearts.Count) {
                Hearts[filled].fillAmount = 0;
            }
        }

        if (blinking > 0) {
            blinkAlpha -= 2f * Time.deltaTime;
            if (blinkAlpha < 0) {
                blinkAlpha = 1;
            }
        } else {
            blinkAlpha = 1;
        }

        if (blinking < BlinkHearts) {
            blinking = BlinkHearts;
        }

        while (blinking > BlinkHearts) {
            blinking--;
            Image image = Hearts[Hearts.Count - 1 - blinking];
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        }

        for (int i = Hearts.Count - 1; i > Hearts.Count - blinking - 1; i--) {
            Image image = Hearts[i];
            image.color = new Color(image.color.r, image.color.g, image.color.b, blinkAlpha);
        }
    }
}
