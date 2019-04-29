using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempFinishUI : SingletonMonoBehaviour<TempFinishUI>
{
    public GameObject cursor;
    protected override void SingletonAwake() {
        this.gameObject.SetActive(false);
    } 
    public void Cursor() {
        cursor.SetActive(false);
    }
    public void Quit() {
        Application.Quit();
    }
}
