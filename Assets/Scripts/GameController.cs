using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Playfield playField;
    public Camera mainCamera;
    public int playerTurn = 0;
    bool hasAiDecisionBeenMadeYet = true;
    //public GUIText whichPlayerText;
    public Text whichPlayerText;
    public GameStateManager gameStateManager;
    public Canvas announcementCanvas;

    public enum PlayerID
    {
        Player1,
        Player2,
        NumPlayers
    }

    // Use this for initialization
    void Start ()
    {
        UpdateWhichPlayerUI();
        Init(1);
    }
    public void Init(int whichBoardToLoad)
    {
        playField.Init(whichBoardToLoad);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public void OnExitButton()
    {
        Scene s = SceneManager.GetActiveScene();
        SceneManager.LoadScene("OpeningScene", LoadSceneMode.Single);
    }

    public void TransitionPlayerTurn()
    {
        if( playField.IsGameBoardFull())
        {
            GameIsADraw();
            SetCurrentPlayer((int)PlayerID.NumPlayers);
        }
        if (playField.TestForFourInARow(playerTurn) == true)
        {
            PlayerWins(); 
            SetCurrentPlayer((int)PlayerID.NumPlayers);
        }
        else
        {
            //PlayerHasATurn();
            SetCurrentPlayer(playerTurn == 0 ? 1 : 0);
            if (playerTurn == (int)PlayerID.Player2)
            {
                hasAiDecisionBeenMadeYet = false;
                StartCoroutine(TakeComputerTurn());
            }
        }
    }

    IEnumerator TakeComputerTurn() 
    {
        WaitForSeconds wfs = new WaitForSeconds(.1f);

        yield return wfs;
        // setup this way to take a little time processing
        // giving the illusion that the AI is thinking
        // todo: add a thinking cursor
        if (playerTurn == (int)PlayerID.Player2 &&
            hasAiDecisionBeenMadeYet == false)
        {
            playField.EvaluateStrategicValue();
            playField.AiPickATile(playerTurn);
            if (playField.TestForFourInARow(playerTurn) == true)
            {
                PlayerLoses();
            }
            else
            {
                TransitionPlayerTurn();
            }
        }

        hasAiDecisionBeenMadeYet = true;
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
        PlayerHasATurn();
        // AI decides on where to play
        //iTween.MoveTo(gameObject, iTween.Hash("y", 0, "delay", 0.2, "time", 1.2, "easetype", iTween.EaseType.easeOutBounce));
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


    public void PlayerLoses()
    {
        SetupAnnouncement("You lose", "Continue", "Exit");
    }
    public void PlayerWins()
    {
        SetupAnnouncement("You win", "Test1", "Test2");
    }
    public void PlayerHasATurn()
    {
        SetupAnnouncement("Your turn", "Test3", "Test4");
    }
    public void GameIsADraw()
    {
        SetupAnnouncement("Game is a draw", "", "");
    }
    public void SetupAnnouncement(string text, string option1, string option2)
    {
        if (announcementCanvas)
        {
            AnnouncementCanvas trans = announcementCanvas.GetComponent<AnnouncementCanvas>() as AnnouncementCanvas;
            trans.SetAnnouncementText(text);
            bool setupCallbackNeeded = false;
            if (option1.Length > 0)
            {
                trans.SetOption1(option1);
                setupCallbackNeeded = true;
            }
            if (option2.Length > 0)
            {
                trans.SetOption2(option2);
                setupCallbackNeeded = true;
            }
            if (setupCallbackNeeded == true)
            {
                trans.Callback += OnAnnouncementClosed;
            }
            trans.Show();
        }
    }
    void OnAnnouncementClosed(string optionPressed)
    {
        AnnouncementCanvas trans = announcementCanvas.GetComponent<AnnouncementCanvas>() as AnnouncementCanvas;
        if(trans)
        {
            trans.Callback -= OnAnnouncementClosed;
        }
    }
}
