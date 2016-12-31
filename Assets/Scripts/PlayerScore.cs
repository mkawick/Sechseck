using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

    public Text playerScore;
	
    public void SetScore(int value)
    {
        if (playerScore)
            playerScore.text = value.ToString();
    }
    public void AddScore(int value)
    {
        if (playerScore)
        {
            int currentValue = Convert.ToInt32(playerScore.text);
            currentValue += value;
            playerScore.text = currentValue.ToString();
        }
    }
}
