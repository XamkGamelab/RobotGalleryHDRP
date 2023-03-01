using UnityEngine;

[RequireComponent(typeof (CharacterController))]
public class FirstPersonPlayer : PlayerCharacter
{    
    public bool CanJump = true;    
    public float JumpSpeed = 8;
    public float StickToGroundForce = 8;
    public float GravityMultiplier = 2;    
    public MouseLook MouseLook;
    
    private bool Jump;
    private bool previouslyGrounded;
    private bool jumping;
    
    private Vector2 MoveInputVector;
    private Vector3 MoveDir = Vector3.zero;    
    private CollisionFlags CollFlags;
    
    protected override void HandleShooting(bool isDown)
    {
        if (isDown) {
            base.HandleShooting(isDown);

        }
    }

    protected override void HandleJumpInput(bool isDown) {

        if (isDown && CanJump) {
            base.HandleJumpInput(isDown);

            // the jump state needs to read here to make sure it is not missed
            if (!Jump)
                Jump = true;
        }
    }

    protected override void Start() {
        base.Start();
        base.UnityCharacterController = GetComponent<CharacterController>();

        jumping = false;            
    }
    
    private void Update() 
    {
        if (MouseLook != null && MouseMove != null)
            MouseLook.UpdateLook(MouseMove.Delta.y, MouseMove.Delta.x);
        
        if (!previouslyGrounded && base.UnityCharacterController.isGrounded) {                              
            MoveDir.y = 0f;
            jumping = false;
        }

        if (!base.UnityCharacterController.isGrounded && !jumping && previouslyGrounded)            
            MoveDir.y = 0f;            

        previouslyGrounded = base.UnityCharacterController.isGrounded;
    }

    private void FixedUpdate()
    {
        float speed;
        GetInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward*MoveInputVector.y + transform.right*MoveInputVector.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, base.UnityCharacterController.radius, Vector3.down, out hitInfo,
                            base.UnityCharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        MoveDir.x = desiredMove.x*speed;
        MoveDir.z = desiredMove.z*speed;
        
        if (base.UnityCharacterController.isGrounded) {
            MoveDir.y = -StickToGroundForce;

            if (Jump) {
                MoveDir.y = JumpSpeed;            
                Jump = false;
                jumping = true;
            }
        }
        else
            MoveDir += Physics.gravity*GravityMultiplier*Time.fixedDeltaTime;

        CollFlags = base.UnityCharacterController.Move(MoveDir*Time.fixedDeltaTime);
        
        MouseLook.UpdateCursorLock();
    }

    private void GetInput(out float speed)
    {      
        // set the desired speed to be walking or running
        speed = Running ? RunSpeed : WalkSpeed;
        MoveInputVector = new Vector2(AxisInput.x, AxisInput.y);

        // normalize input if it exceeds 1 in combined length:
        if (MoveInputVector.sqrMagnitude > 1)
            MoveInputVector.Normalize();
    }
      
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        //dont move the rigidbody if the character is on top of it
        if (CollFlags == CollisionFlags.Below) return;
        
        if (body == null || body.isKinematic)  return;
        
        body.AddForceAtPosition(base.UnityCharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
    }
}

