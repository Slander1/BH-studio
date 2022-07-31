using System.Threading.Tasks;
using Mirror;
using TMPro;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    [SyncVar] public string payerName;
    [SyncVar] public uint score;

    private bool _itIsLongTouch = false;

    private void Awake()
    {
        payerName = "Player + " + netId.ToString();
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

        }
        await Task.Delay(invulnerabilityTime * 1000);
        _itIsLongTouch = false;
        ScoreOnUI();
    }

    [ClientRpc]
    public void ScoreOnUI()
    {
        scoreText.text = score.ToString();
    }
}

