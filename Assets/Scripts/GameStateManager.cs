using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {

    public enum State
    {
        Opening,
        Ending,
        Exit
    }
    public State state;
    // Use this for initialization
    void Start () {
        state = State.Opening;

    }

    public void OnStartGame()
    {
        Scene s = SceneManager.GetActiveScene();
        SceneManager.LoadScene("MainGameScene", LoadSceneMode.Single);
        //s.
       // SceneManager(s);
    }

    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
