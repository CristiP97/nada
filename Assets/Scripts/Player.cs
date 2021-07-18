using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static int lives;
    public static int money;
    public int startMoney = 400;
    public int startLives = 5;

    public event System.Action OnPlayerDeath;
    public event System.Action OnMoneyChange;
    public event System.Action OnLivesChange;

    private bool playerDied;

    private void Start()
    {
        money = startMoney;
        lives = startLives;
        playerDied = false;
    }

    public void LoseLife()
    {
        lives--;

        if (OnLivesChange != null)
        {
            OnLivesChange();
        }

        if (lives <= 0 && !playerDied)
        {
            playerDied = true;
            if (OnPlayerDeath != null)
            {
                OnPlayerDeath();
            }
        }
    }

    public void GainMoney(int amount)
    {
        money += amount;

        if (OnMoneyChange != null)
        {
            OnMoneyChange();
        }

    }

    public void SpendMoney(int amount)
    {
        money -= amount;

        if (OnMoneyChange != null)
        {
            OnMoneyChange();
        }
    }
}
