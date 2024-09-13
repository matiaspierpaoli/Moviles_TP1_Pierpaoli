using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
