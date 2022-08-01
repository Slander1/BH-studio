using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using Mirror;

public class UIController : NetworkBehaviour
{
	[SerializeField] private GameObject VictoryCanvas;
    [SerializeField] private TMP_Text winerText;


    private void OnEnable()
    {
        PlayerInfo.onVictory += VictoryScene;
    }

    private void OnDisable()
    {
        PlayerInfo.onVictory -= VictoryScene;
    }

    [ClientRpc]
    public async void VictoryScene(string winnerName )
    {
        winerText.text = winnerName;
        VictoryCanvas.SetActive(true);
        await Task.Delay(5000);
        VictoryCanvas.SetActive(false);
    }
}

