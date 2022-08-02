using System;
using Mirror;
using UnityEngine;

public class GameLogicController: NetworkBehaviour
{

    private void OnEnable()
    {
        PlayerInfo.onVictory += RestartGame;
    }

    private void OnDisable()
    {
        PlayerInfo.onVictory -= RestartGame;
    }
        
    private void RestartGame(string winerName)
    {
    }

}

