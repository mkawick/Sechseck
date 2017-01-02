using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnouncementCanvas : MonoBehaviour {

    public GameObject childCanvas;
    public Button button1;
    public Button button2;

    string buttonTextPressed;
    public bool waitsForInput;
    public bool expectsButtonPress;

    public delegate void CallbackEventHandler(string something);
    public event CallbackEventHandler Callback;
 
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetAnnouncementText(string text)
    {
        Text textField = childCanvas.GetComponentInChildren<Text>();
        if (textField)
        {
            textField.text = text;
        }
        button1.gameObject.SetActive( false );
        button2.gameObject.SetActive(false);
        waitsForInput = false;
        buttonTextPressed = "";
        expectsButtonPress = false;
    }
    public void SetOption1(string text)
    {
        Text textField = button1.GetComponentInChildren<Text>();
        if(textField)
        {
            textField.text = text;
            button1.gameObject.SetActive(true);
            waitsForInput = true;
            expectsButtonPress = true;
        }
    }
    public void SetOption2(string text)
    {
        Text textField = button2.GetComponentInChildren<Text>();
        if (textField)
        {
            textField.text = text;
            button2.gameObject.SetActive(true);
            waitsForInput = true;
            expectsButtonPress = true;
        }
    }
    public void OnOption1()
    {
        Text textField = button1.GetComponentInChildren<Text>();
        if (textField)
        {
            buttonTextPressed = textField.text;
        }
        waitsForInput = false;
    }

    public void OnOption2()
    {
        Text textField = button2.GetComponentInChildren<Text>();
        if (textField)
        {
            buttonTextPressed = textField.text;
        }
        waitsForInput = false;
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

        while(waitsForInput == true)
        {
            yield return null;
        }
        textField.CrossFadeAlpha(0.0f, fadeTime, false);
        childCanvas.GetComponent<Image>().CrossFadeAlpha(0.0f, fadeTime, false);

        yield return returnTime;

        gameObject.SetActive(false);
        if(expectsButtonPress)
            Callback(buttonTextPressed);
        yield break;
    }

    void OnMouseUp()
    {
        
    }
}
