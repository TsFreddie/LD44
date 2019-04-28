using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void OnDisable() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        RectTransform rect = GetComponent<RectTransform>();
        rect.position = mousePos;
    }
}
