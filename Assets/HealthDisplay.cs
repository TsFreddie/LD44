using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private static HealthDisplay instance;
    public static HealthDisplay I {
        get {
            if (instance) {
                return instance;
            } else {
                return null;
            }
        }
    }
    public GameObject LinePrefab;
    public GameObject HeartPrefab;
    public int Max = 0;
    public int Value = 0;
    public int HeartsPerLine = 10;
    private List<Transform> Lines;
    private List<Image> Hearts;
    private int filled = 0;
    
    void Awake() {
        instance = this;
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
            GameObject newHeart = Instantiate(HeartPrefab);
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
    }
}
