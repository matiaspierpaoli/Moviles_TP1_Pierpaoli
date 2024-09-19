using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    public GameSettings gameSettings; 

    public void SetSinglePlayer(bool isSinglePlayer)
    {
        gameSettings.isSinglePlayerActive = isSinglePlayer;
    }

    public void SetDifficulty(int difficulty)
    {
        gameSettings.currentDifficulty = (Difficulty)difficulty;
    }

    public void SetGameTime(float time, Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gameSettings.easyGameTime = time;
                break;
            case Difficulty.Medium:
                gameSettings.mediumGameTime = time;
                break;
            case Difficulty.Hard:
                gameSettings.hardGameTime = time;
                break;
        }
    }

    public void SetPlayer1Money(int money)
    {
        gameSettings.player1Money = money;
    }

    public void SetPlayer2Money(int money)
    {
        gameSettings.player2Money = money;
    }
}
