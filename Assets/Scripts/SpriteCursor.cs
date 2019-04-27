using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Vector2 mousePos = Input.mousePosition;
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);
    }
}
