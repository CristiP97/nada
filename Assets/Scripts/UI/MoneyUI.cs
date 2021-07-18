using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Text moneyText;

    public void Start()
    {
        FindObjectOfType<Player>().OnMoneyChange += UpdatePlayerMoney;
        Invoke("UpdatePlayerMoney", 0.1f); // this is done to make sure that the Player class has already instantiated the lives
    }

    private void UpdatePlayerMoney()
    {
        moneyText.text = "Money: " + Player.money;
    }
}
