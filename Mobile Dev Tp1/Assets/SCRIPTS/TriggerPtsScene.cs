using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPtsScene : MonoBehaviour
{
    [SerializeField] private string playerTag;

    [SerializeField] private GameObject player1GO;
    [SerializeField] private GameObject player2GO; 
    
    [SerializeField] private GameObject waitStateCanvasPlayer1;
    [SerializeField] private GameObject waitStateCanvasPlayer2;
    
    [SerializeField] private GameObject virtualJoystickPlayer1;
    [SerializeField] private GameObject virtualJoystickPlayer2;
    
    [SerializeField] private int player1Id;
    [SerializeField] private int player2Id;

    private bool player1Passed;
    private bool player2Passed;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instancia.gameSettings.isSinglePlayerActive)
        {
            GameManager.Instancia.ChangeState(new GameOverState());
        }    
        else
        {
            if (other.gameObject.tag == playerTag)
            {
                if (other.gameObject.GetComponent<Player>().IdPlayer == player1Id)
                {
                    player1Passed = true;
                    waitStateCanvasPlayer1.SetActive(true);
                    player1GO.SetActive(false);
                    virtualJoystickPlayer1.SetActive(false);
                }
                else if (other.gameObject.GetComponent<Player>().IdPlayer == player2Id)
                {
                    player2Passed = true;
                    waitStateCanvasPlayer2.SetActive(true);
                    player2GO.SetActive(false);
                    virtualJoystickPlayer2.SetActive(false);
                }
            }

            if (player1Passed && player2Passed)
            {
                GameManager.Instancia.ChangeState(new GameOverState());
            }
        }
    }
}
