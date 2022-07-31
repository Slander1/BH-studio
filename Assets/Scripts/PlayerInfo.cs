using System.Threading.Tasks;
using Mirror;
using TMPro;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    [SyncVar] public int payerName;
    [SyncVar] public uint score;

    private bool _itIsLongTouch = false;

    public async void scoreOnUI(int invulnerabilityTime)
    {
        if (_itIsLongTouch)
            return;
        _itIsLongTouch = true;
        score++;
        await Task.Delay(invulnerabilityTime * 1000);
        scoreText.text = score.ToString();
        _itIsLongTouch = false;
    }
}

