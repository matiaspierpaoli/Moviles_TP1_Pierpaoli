using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DatosPartida;

public enum Difficulty
{
    Easy = 0,
    Medium,
    Hard
}

[CreateAssetMenu(fileName = "NewGameSettings", menuName = "Settings/Game Settings")]
public class GameSettings : ScriptableObject
{
    public bool isSinglePlayerActive;
    public Difficulty currentDifficulty;

    public float easyGameTime;
    public float mediumGameTime;
    public float hardGameTime;

    public int player1Money;
    public int player2Money;

    public Lados LadoGanadaor;

}
