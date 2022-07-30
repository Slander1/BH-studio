using Mirror;
using UnityEngine;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;


    public void OnHostBtn()
    {
        networkManager.StartHost();
    }

    public void SetIP(string ip)
    {
        networkManager.networkAddress = ip;
    }

    public void SetName(string name)
    {
        //PlayerLocalData.playerName = name;
    }

    public void OnConnectBtn()
    {
        networkManager.StartClient();
    }

    public void OnExitBtn()
    {
        Application.Quit();
    }
}