using Mirror;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : NetworkBehaviour
{
    [SerializeField] private float movingSpeed = 1f;
    [SerializeField] private float mouseSens = 5f;
    [SerializeField] private Renderer renderer;
    [SerializeField] private Color damagedColor;
    [SerializeField] private int invulnerabilityTime;
    [SerializeField] private int ImpulseCooldown;
    [SerializeField] private GameObject camera;
<<<<<<< HEAD
=======
    [SerializeField] private int impulseTouchToWin = 5;


    [SerializeField] private TMP_Text scoretext;
    [SerializeField] private TMP_Text playerName;

>>>>>>> e097f2699b4e33941e05850ee179698106c76834

    private Rigidbody _rigidbody;
    private Quaternion originRotation;
    private float angleVertical;
    private bool isImpulsed;

    [SyncVar] public bool isInvulnerability;
    [SyncVar] private Color _color;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
<<<<<<< HEAD
        originRotation = transform.rotation;
        //Cursor.lockState = CursorLockMode.Locked;
        color = new Color(Random.Range(0,1f),
            Random.Range(0, 1f),
            Random.Range(0, 1f));
    }
=======
        _originRotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
        /*playerName.text = (PlayerLocalData.playerName == "" || PlayerLocalData.playerName == null)?
            Random.Range(10,1999).ToString(): PlayerLocalData.playerName;*/
     }
>>>>>>> e097f2699b4e33941e05850ee179698106c76834

    private void Start()
    {
        _color = new Color(Random.Range(0, 255), Random.Range(0, 255),
            Random.Range(0, 255));
        Debug.Log(_color);
        renderer.material.color = _color;
    }

    private void MoveToDirection(Vector3 direction)
    {
        _rigidbody.AddRelativeForce(direction * movingSpeed, ForceMode.Acceleration);
    }

    private void LateUpdate()
    {
<<<<<<< HEAD
        camera.gameObject.SetActive(isLocalPlayer);
        renderer.material.color = isInvulnerability ? damagedColor : color;
=======
        renderer.material.color = isInvulnerability ? damagedColor : _color;

>>>>>>> e097f2699b4e33941e05850ee179698106c76834
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
                CmdFireTest();

            //angleHorizontal += Input.GetAxis("Mouse X") * mouseSens;
            angleVertical += Input.GetAxis("Mouse Y") * mouseSens;
            var rotationX = Quaternion.AngleAxis(-angleVertical, Vector3.up);

            transform.rotation = originRotation * rotationX;
        }
    }

    [Command]
    private void CmdFireTest()
    {
        CmdFire();
    }

    [ClientRpc]
    private async void CmdFire()
    {
        if(isImpulsed)
            return;
        _rigidbody.AddRelativeForce(Vector3.forward * (movingSpeed * 600), ForceMode.Acceleration);
        isImpulsed = true;
        await Task.Delay(ImpulseCooldown * 1000);
        isImpulsed = false;
    }

    private async void OnTriggerEnter(Collider other)
    {
        var otherComponent = other.GetComponentInParent<CharacterController>();
        if (otherComponent != null && otherComponent != this && isImpulsed)
        {
            otherComponent.isInvulnerability = true;
            await Task.Delay(invulnerabilityTime * 1000);
            otherComponent.isInvulnerability = false;
<<<<<<< HEAD
=======
            _playerScore++;
            Debug.Log(_playerScore.ToString() + " " + name + " " + other.name);
            scoretext.text = _playerScore.ToString();
>>>>>>> e097f2699b4e33941e05850ee179698106c76834
        }
    }
}