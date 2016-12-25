using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacingLabel : MonoBehaviour {

    Camera referenceCamera;
    public TextMesh label;

    void Awake()
    {
        // if no camera referenced, grab the main camera
        if (!referenceCamera)
            referenceCamera = Camera.main;
    }

    void Start()
    {
        //label//label
        //transform.localEulerAngles = new Vector3(180, 0, 0);
    }

    public void Init(int value)
    {
        label.text = "<" + value + ">";
    }
    public void Init(int x, int y)
    {
        label.text = "[" + x + "," + y + "]";
    }

    void Update()
    {
        transform.LookAt(transform.position + referenceCamera.transform.rotation * Vector3.forward,
            referenceCamera.transform.rotation * Vector3.up);
    }
}
