using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Text healthText;
    public Text coinsText;
    public GameObject currentPlayer;
    public Health currentHealth;


    public void Awake()
    {
        instance = this;
        AttachToPlayer(currentPlayer.gameObject);
        
        PlayerController.addCoin += DrawCoins;
    }


    public void DrawHealth()
    {
        string hString = "";
        for (int i = 0; i < currentHealth.health; i++)
        {
            hString += '0';
        }
        healthText.text = hString;
    }

    public void DrawCoins()
    {
        Debug.Log("drawcoin");
        coinsText.text = "Coins x" + PlayerController.coins;
    }

    void SwitchPlayers(Walking player)
    {
        
    }

    public void AttachToPlayer(GameObject player)
    {
        currentPlayer = player;
        currentHealth = player.GetComponent<Health>();
        DrawHealth();
        currentHealth.damaged += DrawHealth;

    }

    private void OnDestroy()
    {
        PlayerController.addCoin -= DrawCoins;
    }

}
