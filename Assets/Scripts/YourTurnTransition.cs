using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YourTurnTransition : MonoBehaviour {

    public GameObject childCanvas;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetAnnouncementText(string text)
    {
        Text textField = childCanvas.GetComponentInChildren<Text>();
        textField.text = text;
    }
    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(CrossFadeIn());
    }

    IEnumerator CrossFadeIn()
    {
        WaitForSeconds returnTime = new WaitForSeconds(0.5f);
        float fadeTime = 0.5f;

        gameObject.SetActive(true);

        Text textField = childCanvas.GetComponentInChildren<Text>();
        textField.GetComponent<CanvasRenderer>().SetAlpha(0f);
        textField.CrossFadeAlpha(1.0f, fadeTime, false);
        childCanvas.GetComponent<CanvasRenderer>().SetAlpha(0f);
        childCanvas.GetComponent<Image>().CrossFadeAlpha(1.0f, fadeTime, false);

        yield return returnTime;

        yield return returnTime;// pause

        textField.CrossFadeAlpha(0.0f, fadeTime, false);
        childCanvas.GetComponent<Image>().CrossFadeAlpha(0.0f, fadeTime, false);

        yield return returnTime;

        gameObject.SetActive(false);
        yield break;
    }

}
