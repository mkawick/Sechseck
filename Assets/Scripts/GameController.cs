using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {

    public Playfield playField;
    public Camera mainCamera;
    public int playerTurn = 0;
    bool hasAiDecisionBeenMadeYet = true;
    //public GUIText whichPlayerText;
    public Text whichPlayerText;

    public Canvas announcementCanvas;

    public enum PlayerID
    {
        Player1,
        Player2,
        NumPlayers
    }

    // Use this for initialization
    void Start () {
        UpdateWhichPlayerUI();
    }
	
	// Update is called once per frame
	void Update () {
        if (playerTurn == (int)PlayerID.Player2 && hasAiDecisionBeenMadeYet == false)
        {
            playField.EvaluateStrategicValue();
            playField.AiPickATile(playerTurn);
            if (playField.TestForFourInARow(playerTurn) == true)
            {
                SetupAnnouncement("You lose");
            }
            else 
                TransitionPlayerTurn();
        }

        hasAiDecisionBeenMadeYet = true;
    }

    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void TransitionPlayerTurn()
    {
        if( playField.IsGameBoardFull())
        {
            SetupAnnouncement("Game is a draw");
            SetCurrentPlayer((int)PlayerID.NumPlayers);
        }
        if (playField.TestForFourInARow(playerTurn) == true)
        {
            SetupAnnouncement("You win");
            SetCurrentPlayer((int)PlayerID.NumPlayers);
        }
        else {

            SetCurrentPlayer(playerTurn == 0 ? 1 : 0);

            if (playerTurn == (int)PlayerID.Player2)
                hasAiDecisionBeenMadeYet = false;
        }
        if (playField.IsGameBoardFull())
        {
            SetupAnnouncement("Game is a draw");
            SetCurrentPlayer((int)PlayerID.NumPlayers);
        }
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
        playField.TestForFourInARow(playerTurn);
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
    public void OnEndTurn()
    {
        SetupAnnouncement("Your turn");
        // AI decides on where to play
        //iTween.MoveTo(gameObject, iTween.Hash("y", 0, "delay", 0.2, "time", 1.2, "easetype", iTween.EaseType.easeOutBounce));
    }

    public void SetupAnnouncement(string text)
    {
        if (announcementCanvas)
        {
            YourTurnTransition trans = announcementCanvas.GetComponent<YourTurnTransition>() as YourTurnTransition;
            trans.SetAnnouncementText("Your turn");
            trans.Show();
        }
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
