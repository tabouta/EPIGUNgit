using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMotor : MonoBehaviour
{
   
    // Start is called before the first frame update
    [SerializeField]
    private Camera cam;
    private Vector3 velocity;
    private Vector3 rotation;
    private Vector3 cameraRotation;
    public static Rigidbody rb;
    [SerializeField] 
    private float jumpForce = 8f;
    private bool grounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }
    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
        Jump();
    }

    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

    }
    private void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        cam.transform.Rotate(-cameraRotation);
        
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }
	
    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }
    
    
}
