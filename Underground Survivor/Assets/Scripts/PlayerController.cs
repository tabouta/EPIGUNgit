using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PauseMenu pause;
    [SerializeField] 
    private float speed = 3f;
    [SerializeField] 
    private float jumpForce = 8f;
    private const float SPRINTSPEED = 2f;
    private bool isSprinting;

    [SerializeField] 
    private float MouseSensitivityX = 3f; 
    [SerializeField] 
    private float MouseSensitivityY = 3f;

    private bool grounded;
    private PlayerMotor motor;

    [SerializeField]
    private Animator anim;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //Calculer la vélocité/vitesse du mouvement du personnage
        if(pause.InPause)
        {
            return;
        }
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");
        if(zMov != 0)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                isSprinting = true;
                anim.SetBool("IfRunning",true);

            }
            else
            {
                isSprinting = false;
                anim.SetBool("IfWalking",true);
                anim.SetBool("IfRunning",false);
            }
            
        
        }
        else
        {
            anim.SetBool("IfRunning",false);
            anim.SetBool("IfWalking",false);
        }

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed *(isSprinting?SPRINTSPEED:1);


        motor.Move(velocity);

		//On calcule la rotation du joueur avec un Vector3
		float yRot = Input.GetAxisRaw("Mouse X");
		
		Vector3 rotation = new Vector3(0,yRot,0) * MouseSensitivityX;
		
		motor.Rotate(rotation);
        //On calcule la rotation de la caméra avec un Vector3
		float xRot = Input.GetAxisRaw("Mouse Y")  * MouseSensitivityY;
		Vector3 cameraRotation = new Vector3(xRot,0,0);
		
		motor.RotateCamera(cameraRotation);

		//On gère le système de saut
		Jump();
    }
    private bool isJumping;
    private IEnumerator waiting()
    {
        yield return new WaitForSeconds(1);
        isJumping = false;
        anim.SetBool("IfJumping",false);
    }
    void Jump()
    {
	    if (Input.GetKeyDown(KeyCode.Space) && grounded && !isJumping)
	    {
            isJumping = true;
            anim.SetBool("IfJumping",true);
		    PlayerMotor.rb.AddForce(transform.up * jumpForce);
            StartCoroutine("waiting");
	    }
    }
	
    public void SetGroundedState(bool _grounded)
    {
	    grounded = _grounded;
    }
}
