using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [SerializeField] private float movingSpeed = 1f;
    [SerializeField] private float rotationSpeed;

    private Camera _camera;
    private Vector3 offset = new Vector3(0, 2, -5);


    Quaternion originRotation;
    float angleHorizontal;
    float angleVertical;
    float mouseSens = 5;
    float stopFactor = 8;


    void Awake()
    {
        _camera = Camera.main;

        originRotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void MoveRight()
    {
        transform.position += new Vector3(movingSpeed, 0, 0);
    }

    private void MoveLeft()
    {
        transform.position += new Vector3(-movingSpeed, 0, 0);
    }

    private void MoveUp()
    {
        transform.position += new Vector3(0, 0, movingSpeed);
    }

    private void MoveDown()
    {
        transform.position += new Vector3(0, 0, -movingSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            MoveRight();
        if (Input.GetKeyDown(KeyCode.A))
            MoveLeft();
        if (Input.GetKeyDown(KeyCode.W))
            MoveUp();
        if (Input.GetKeyDown(KeyCode.S))
            MoveDown();

        _camera.transform.position = Vector3.Lerp(_camera.transform.position, transform.position
            + offset, Time.deltaTime * movingSpeed);

        angleHorizontal += Input.GetAxis("Mouse X") * mouseSens;
        angleVertical += Input.GetAxis("Mouse Y") * mouseSens;

        angleVertical = Mathf.Clamp(angleVertical, -60, 60);

        Quaternion rotationY = Quaternion.AngleAxis(angleHorizontal, Vector3.up);
        Quaternion rotationX = Quaternion.AngleAxis(-angleVertical, Vector3.right);

        _camera.transform.rotation = originRotation * rotationY * rotationX;


    }
}
