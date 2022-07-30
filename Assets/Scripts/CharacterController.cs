using Mirror;
using TMPro;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Material))]
public class CharacterController : NetworkBehaviour
{
    [SerializeField] private float movingSpeed = 1f;
    [SerializeField] private float mouseSens = 5f;
    [SerializeField] private Renderer renderer;
    [SerializeField] private Color damagedColor;
    [SerializeField] private int invulnerabilityTime;
    [SerializeField] private int ImpulseCooldown;
    [SerializeField] private GameObject camera;
    [SerializeField] private int impulseTouchToWin = 5;


    [SerializeField] private TMP_Text scoretext;
    [SerializeField] private TMP_Text playerName;


    private Rigidbody _rigidbody;
    private Quaternion _originRotation;
    private float _angleHorizontal;
    private bool _isImpulsed;


    [SyncVar] private int _playerScore;
    [SyncVar] public bool isInvulnerability;
    [SyncVar] private Color _color;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _originRotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
        /*playerName.text = (PlayerLocalData.playerName == "" || PlayerLocalData.playerName == null)?
            Random.Range(10,1999).ToString(): PlayerLocalData.playerName;*/
     }

    private void Start()
    {
        _color = new Color(Random.Range(0, 255), Random.Range(0, 255),
            Random.Range(0, 255));
        Debug.Log(_color);
        renderer.material.color = _color;
    }

    private void MoveToDirection(Vector3 direction)
    {
        _rigidbody.AddRelativeForce(direction * movingSpeed, ForceMode.VelocityChange);
    }

    private void LateUpdate()
    {
        renderer.material.color = isInvulnerability ? damagedColor : _color;

        if (isLocalPlayer)
        {
            var keyDirection = Vector3Int.zero;

            if (Input.GetKey(KeyCode.D))
                keyDirection = Vector3Int.right;
            if (Input.GetKey(KeyCode.A))
                keyDirection = Vector3Int.left;
            if (Input.GetKey(KeyCode.W))
                keyDirection = Vector3Int.forward;
            if (Input.GetKey(KeyCode.S))
                keyDirection = Vector3Int.back;

            if (keyDirection != Vector3Int.zero)
                MoveToDirection(keyDirection);

            if (Input.GetKeyDown(KeyCode.Mouse0))
                CmdImpulse();
        }
            _angleHorizontal += Input.GetAxis("Mouse Y") * mouseSens;
            var rotationX = Quaternion.AngleAxis(-_angleHorizontal, Vector3.up);

            transform.rotation = _originRotation * rotationX;
    }

    [Command]
    private void CmdImpulse()
    {
        Impulse();
    }

    [ClientRpc]
    private async void Impulse()
    {
        if(_isImpulsed)
            return;
        _rigidbody.AddRelativeForce(Vector3.forward * (movingSpeed * 600), ForceMode.VelocityChange);
        _isImpulsed = true;
        await Task.Delay(ImpulseCooldown * 1000);
        _isImpulsed = false;
    }

    private async void OnTriggerEnter(Collider other)
    {
        var otherComponent = other.GetComponentInParent<CharacterController>();
        if (otherComponent != null && otherComponent != this && _isImpulsed)
        {
            otherComponent.isInvulnerability = true;
            await Task.Delay(invulnerabilityTime * 1000);
            otherComponent.isInvulnerability = false;
            _playerScore++;
            Debug.Log(_playerScore.ToString() + " " + name + " " + other.name);
            scoretext.text = _playerScore.ToString();
        }
    }
}