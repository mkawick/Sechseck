using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {

    public Playfield playField;
    public Camera mainCamera;
    public int playerTurn = 0;
    //public GUIText whichPlayerText;
    public Text whichPlayerText;

    // Use this for initialization
    void Start () {
        UpdateWhichPlayerUI();
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OnResetButton()
    {
        playField.ResetToTestDefault();
        playerTurn = 0;
        UpdateWhichPlayerUI();
        playField.Clear4InARow();
    }
    public void OnCircleButton()
    {
        playField.SetToCircle();
        playerTurn = 0;
        UpdateWhichPlayerUI();
        playField.Clear4InARow();
    }
    public void OnShowLabel()
    {
        playField.ShowLabels();
    }
    public void OnLinearMap()
    {
        playField.ToggleLinearMap();
        playField.Clear4InARow();
    }

    public void OnFourInARow()
    {
        playField.Setup4InARow(playerTurn);
    }

    public void DragCamera( float delta)
    {
        if (mainCamera)
        {
            //Vector3 dist = mainCamera.transform.position;
            //mainCamera.transform.position = Vector3.zero;
            //mainCamera.transform.rotation += Vector3
            mainCamera.transform.RotateAround(Vector3.zero, Vector3.up, (float)(0.5 * delta));
        }
    }

    public void OnEvaluate()
    {
        playField.EvaluateStrategicValue();
    }

    public void NextPlayer()
    {
        SetCurrentPlayer(playerTurn==0?1:0);
    }
    public void SetCurrentPlayer(int index)
    {
        if (index > -1 && index < 2)
        {
            playerTurn = index;
            UpdateWhichPlayerUI();
        }
    }
    public int GetCurrentPlayer()
    {
        return playerTurn;
    }

    private void UpdateWhichPlayerUI()
    {
        int num = playerTurn + 1;
        if (whichPlayerText)
            whichPlayerText.text = "Player: " + num;
    }
}
