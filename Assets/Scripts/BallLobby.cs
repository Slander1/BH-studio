using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class BallLobby : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 1f;

    private Rigidbody _rigidbody;
    private Vector3 _direction = Vector3.left;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.x >= 10)
            _direction = Vector3Int.left;
        else if (transform.position.x <= -10)
            _direction = Vector3Int.right;

        _rigidbody.AddRelativeForce(_direction * movingSpeed, ForceMode.VelocityChange);
    }
}
