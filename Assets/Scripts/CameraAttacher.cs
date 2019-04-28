using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAttacher : MonoBehaviour
{
    public Transform DefaultTarget;
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(DefaultTarget, false);
        transform.localPosition = Vector3.forward * -10f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
