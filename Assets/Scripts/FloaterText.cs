using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterText : MonoBehaviour {

    public Color color= new Color(0.8f,0.8f,0f,1.0f);
    public float scroll  = 0.2f;  // scrolling velocity
    public float duration = 1.5f; // time to die
    private float alpha ;
    public GameObject TextFloater;
    Camera referenceCamera;
    // Use this for itialization
    void Start()
    {
        if (!referenceCamera)
            referenceCamera = Camera.main;
        Renderer rend = gameObject.GetComponent<Renderer>();
        //rend.material.SetColor("_Color", Color.green);

        //games
        //gameObject.GetComponent<GUIText>().material.color = color; // set text color
        alpha = 1;
        iTween.MoveTo(gameObject, iTween.Hash("y", 5, "time", 5.0, "easetype", iTween.EaseType.easeOutSine));
    }

    // Update is called once per frame
    void Update () {
        if (alpha > 0)
        {
            alpha -= Time.deltaTime / duration;
            Color temp = color;
            temp.a = alpha;
            TextFloater.GetComponent<TextMesh>().color = temp;

            transform.LookAt(transform.position + referenceCamera.transform.rotation * Vector3.forward,
            referenceCamera.transform.rotation * Vector3.up);
        }
        else {
            Destroy(gameObject); // text vanished - destroy itself
        }
    }

    public void SetText(string text)
    {
        TextFloater.GetComponent<TextMesh>().text = text;
    }
}
