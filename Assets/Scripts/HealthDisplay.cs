﻿using System.Collections;
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
    private int blinking = 0;
    private float blinkAlpha = 1;
    private List<Transform> Lines;
    private List<Image> Hearts;
    private int filled = 0;
    public float NextHeart = 0;
    
    protected override void SingletonAwake() {
        Lines = new List<Transform>();
        Hearts = new List<Image>();
    }

    public int BlinkHearts {
        get {
            return blinking;
        }
        set {
            blinking = value;
            foreach (Image image in Hearts) {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
            }
        }
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
            newLine.transform.SetParent(transform, false);
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
            newHeart.transform.SetParent(Lines[line], false);
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

        for (int i = 0; i < Hearts.Count; i++) {
            if (i < Value) {
                Hearts[i].fillAmount = 1;
            } else {
                Hearts[i].fillAmount = 0;
            }
        }

        /*
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

        if (filled < Hearts.Count) {
            Hearts[filled].fillAmount = NextHeart;
        }
        */
        if (blinking > 0) {
            blinkAlpha -= 2f * Time.unscaledDeltaTime;
            if (blinkAlpha < 0) {
                blinkAlpha = 1;
            }
        } else {
            blinkAlpha = 1;
        }

        for (int i = 0; i < blinking; i++) {
            Image image = Hearts[Value - i - 1];
            image.color = new Color(image.color.r, image.color.g, image.color.b, blinkAlpha);
        }
    }
}
