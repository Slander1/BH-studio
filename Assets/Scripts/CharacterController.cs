using System;
using Mirror;
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

    private Rigidbody _rigidbody;
    private Quaternion _originRotation;
    private float _angleHorizontal;
    private float _angleVertical;
    private bool _isImpulsed;
    private Material _characterMaterial;

    [SyncVar] public bool isInvulnerability;
    [SyncVar] public Color color;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _originRotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
        _characterMaterial = GetComponent<Renderer>().material;
        _characterMaterial.color = new Color(UnityEngine.Random.Range(0, 255), UnityEngine.Random.Range(0, 255),
            UnityEngine.Random.Range(0, 255));
     }

    private void MoveToDirection(Vector3 direction)
    {
        _rigidbody.AddRelativeForce(direction * movingSpeed, ForceMode.Acceleration);
    }

    private void LateUpdate()
    {
        camera.gameObject.SetActive(isLocalPlayer);
        renderer.material.color = isInvulnerability ? damagedColor : Color.white;

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
        }
            //angleHorizontal += Input.GetAxis("Mouse X") * mouseSens;
            _angleVertical += Input.GetAxis("Mouse Y") * mouseSens;
            var rotationX = Quaternion.AngleAxis(-_angleVertical, Vector3.up);

            transform.rotation = _originRotation * rotationX;
     
    }

    [Command]
    private void CmdFireTest()
    {
        CmdFire();
    }

    [ClientRpc]
    private async void CmdFire()
    {
        if(_isImpulsed)
            return;
        _rigidbody.AddRelativeForce(Vector3.forward * (movingSpeed * 600), ForceMode.Acceleration);
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
        }
    }
}