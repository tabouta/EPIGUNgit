using UnityEngine;
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float speed = 3f;
    [SerializeField] 
    private float jumpForce = 8f;


    [SerializeField] 
    private float MouseSensitivityX = 3f; 
    [SerializeField] 
    private float MouseSensitivityY = 3f;

    private bool grounded;
    private PlayerMotor motor;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        //Calculer la vélocité/vitesse du mouvement du personnage
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.Move(velocity);

		//On calcule la rotation du joueur avec un Vector3
		float yRot = Input.GetAxisRaw("Mouse X");
		
		Vector3 rotation = new Vector3(0,yRot,0) * MouseSensitivityX;
		
		motor.Rotate(rotation);
        //On calcule la rotation de la caméra avec un Vector3
		float xRot = Input.GetAxisRaw("Mouse Y");
		
		Vector3 cameraRotation = new Vector3(xRot,0,0) * MouseSensitivityY;
		
		motor.RotateCamera(cameraRotation);
		//On gère le système de saut
		Jump();
    }

    void Jump()
    {
	    if (Input.GetKeyDown(KeyCode.Space) && grounded)
	    {
		    PlayerMotor.rb.AddForce(transform.up * jumpForce);
	    }
    }
	
    public void SetGroundedState(bool _grounded)
    {
	    grounded = _grounded;
    }
}
