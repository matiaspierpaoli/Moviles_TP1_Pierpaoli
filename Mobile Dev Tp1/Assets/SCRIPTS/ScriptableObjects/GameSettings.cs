using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameSettings", menuName = "Settings/Game Settings")]
public class GameSettings : ScriptableObject
{
    public bool isSinglePlayerActive;
}
