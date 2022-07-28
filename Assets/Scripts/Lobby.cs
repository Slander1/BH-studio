using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
[HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager-hud")]
public class Lobby : NetworkManagerHUD
{
    [SerializeField] private Camera camera;

    private void Update()
    {
        camera.gameObject.SetActive(!NetworkClient.isConnected);
    }
}