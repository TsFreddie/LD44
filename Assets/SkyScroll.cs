using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyScroll : MonoBehaviour
{
    public Transform BottomLeft;
    public Transform TopRight;
    public GameObject Sky;
    private float top;
    private float bottom;
    private float left;
    private float right;
    private float width;
    private float height;

    // Start is called before the first frame update
    void Start()
    {
        top = TopRight.position.y;
        bottom = BottomLeft.position.y;
        left = BottomLeft.position.x;
        right = TopRight.position.x;
        width = right - left;
        height = top - bottom;
    }

    // Update is called once per frame
    void Update()
    {
        float yPercent = (Camera.main.transform.position.y - bottom) / height;
        float xPercent = (Camera.main.transform.position.x - left) / width;
        Sky.transform.localPosition = new Vector3(-(xPercent * 16 - 8), -(yPercent * 30 - 15), 10);
    }
}
