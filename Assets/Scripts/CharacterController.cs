using System;
using Mirror;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : NetworkBehaviour
{
    [Header("Moving Setting")]
    [SerializeField] private float movingSpeed = 1f;
    [SerializeField] private float mouseSens = 5f;


    [Header("GameObject Setting")]
    [SerializeField] private Camera camera;
    [SerializeField] private Renderer renderer;
    [SyncVar] public bool isInvulnerability;
    [SyncVar] public Color color;


    [Header("Impulse Setting")]
    [SerializeField] private Color colorAfterDamaged;
    [SerializeField] private int invulnerabilityTime;
    [SerializeField] private int ImpulseCooldown;
    [SerializeField] private int  powerImpulse;



    private Rigidbody _rigidbody;
    private Quaternion originRotation;
    private float angleHorizontal;
    private bool isImpulsed;

    


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        originRotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
        color = new Color(UnityEngine.Random.Range(0,1f),
            UnityEngine.Random.Range(0, 1f),
            UnityEngine.Random.Range(0, 1f));

        if (!isLocalPlayer)
            camera.gameObject.SetActive(false);
    }

    private void MoveToDirection(Vector3 direction)
    {
        _rigidbody.AddRelativeForce(direction * movingSpeed, ForceMode.Acceleration);
    }

    private void LateUpdate()
    {

        renderer.material.color = isInvulnerability ? colorAfterDamaged : color;
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

            if (Input.GetKeyDown(KeyCode.Escape))
                CursorUnlock();

            angleHorizontal += Input.GetAxis("Mouse Y") * mouseSens;
            var rotationX = Quaternion.AngleAxis(-angleHorizontal, Vector3.up);
            transform.rotation = originRotation * rotationX;
        }
    }

    private async void CursorUnlock()
    {
        Cursor.lockState = CursorLockMode.None;
        await Task.Delay(3000);
        Cursor.lockState = CursorLockMode.Locked;
    }

    [Command]
    private void CmdImpulse()
    {
        Impulse();
    }

    [ClientRpc]
    private async void Impulse()
    {
        if(isImpulsed)
            return;
        _rigidbody.AddRelativeForce(Vector3.forward * (powerImpulse * 600), ForceMode.Acceleration);
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
        }
    }
}