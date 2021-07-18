using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesLeftUI : MonoBehaviour
{
    public Text livesLeftText;

    public void Start()
    {
        FindObjectOfType<Player>().OnLivesChange += UpdatePlayerLives;
        Invoke("UpdatePlayerLives", 0.1f); // this is done to make sure that the Player class has already instantiated the lives
    }

    private void UpdatePlayerLives()
    {
        livesLeftText.text = "Lives left " + Player.lives;
    }
}
