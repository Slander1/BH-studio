using System;
using System.Threading.Tasks;
using Mirror;
using TMPro;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{

    [SerializeField] private TMP_Text scoreText;

    [SyncVar] public string playerName;
    [SyncVar] public uint score;

    public static Action<string> onVictory;

    private bool _itIsLongTouch = false;

    private void Awake()
    {
        playerName = "Player + " + netId.ToString();
    }

    [Command]
    public async void scoreUp(int invulnerabilityTime)
    {
        if (_itIsLongTouch)
            return;
        _itIsLongTouch = true;
        score++;
        if (score == 3)
        {
            onVictory?.Invoke(playerName);
            score = 0;
        }
        await Task.Delay(invulnerabilityTime * 1000);
        _itIsLongTouch = false;
        ScoreOnUI();
    }

    [ClientRpc]
    public void ScoreOnUI()
    {
        scoreText.text = (score == 0)? "" : score.ToString();
    }

    
}

