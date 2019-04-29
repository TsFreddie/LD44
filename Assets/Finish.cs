using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (((Vector2)Character.I.transform.position - (Vector2)transform.position).magnitude < 8) {
            Time.timeScale = 0;
            TempFinishUI.I.Cursor();
            TempFinishUI.I.gameObject.SetActive(true);
        }
    }
}
