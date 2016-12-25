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

    public void Show()
    {
        gameObject.SetActive(true);
        // fade to transparent over 500ms.
        //CrossFadeAlpha(0.0f, 0.05f, false);
        //Graphic gr = parentCanvas.GetComponent<Graphic>() as Graphic;
        //gr.CrossFadeColor(Color.white, 1.0f, false, true);
        //gr.CrossFadeColor(Color.black, 1.0f, false, true);

        //Graphic gr = childCanvas.GetComponent<Graphic>() as Graphic;
        //iTween.FadeTo(gameObject, 1.0f, 5.0f);
        iTween.MoveTo(gameObject, iTween.Hash("alpha", 0, "amount", 1.0, "delay", 0.2, "time", 1.2, "easetype", iTween.EaseType.easeOutBounce, "oncomplete", "EndOfFadeIn"));
        //thatButton.GetComponent<Image>().CrossFadeAlpha(0.1f, 2.0f, false);

        // and back over 500ms.
        //text.CrossFadeAlpha(1.0f, 0.05f, false);
        //iTween.MoveTo(gameObject, iTween.Hash("y", 0, "delay", 0.2, "time", 1.2, "easetype", iTween.EaseType.easeOutBounce));
    }

    void EndOfFadeIn()
    {

    }
}

class FadeMaterials : MonoBehaviour
{
 /*   public void FadeOut()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 1.0f, "to", 0.0f,
            "time", 3f, "easetype", "linear",
            "onupdate", "setAlpha");
    }
    public void FadeIn()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 0f, "to", 1f,
            "time", 3f, "easetype", "linear",
            "onupdate", "setAlpha");
    }*/

  /*  public void setAlpha(float newAlpha)
    {
        foreach (Material mObj in renderer.materials)
        {
            mObj.color = new Color(
                mObj.color.r, mObj.color.g,
                mObj.color.b, newAlpha);
        }
    }*/

}