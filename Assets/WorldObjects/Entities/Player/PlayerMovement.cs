using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    //current state of player
    public PlayerState state;

    //current sprint timer and max value
    private float sprintTime;
    private float maxSprintTime;

    //height of collider when standing and crouching
    private float boxStandHeight;
    private float boxCrouchHeight;

    //height of camera when standing and crouching
    private float cameraStandHeight;
    private float cameraCrouchHeight;

    //player box collider and camera
    public BoxCollider box;
    public Camera camera;

    //prevents player changes happening every update
    public bool walking;
    public bool crouching;
    public bool sprinting;
    public bool canCrouch = false;
	public bool canJump = true;
    //used to play breathing sound
    private bool breathing;
    public bool recharging;

    //player motor and controller
    public CharacterMotor motor;
    public CharacterController controller;

	private bool inWater = false;

	// Use this for initialization
	void Start () 
	{
        //set timer
        sprintTime = 6.0F;
        maxSprintTime = 6.0F;

        //set player collider heights
        boxStandHeight = box.size.y;
        boxCrouchHeight = 1.125f;

        //set player camera heights
        cameraStandHeight = camera.transform.localPosition.y;
        cameraCrouchHeight = 0.30857f;

		canCrouch = false;
	}
	
	// Update is called once per frame
    void Update() {
        //recharge sprint if not trying to sprint
        if (!Input.GetKey(KeyCode.LeftShift) && sprintTime < maxSprintTime) {
            sprintTime += Time.deltaTime;
        }

        //if cannot sprint, start breathing sound
        if (sprintTime < 0) {
            if (breathing) {
                breathing = false;

                GetComponent<Player>().breathingSound.Play();
            }
        }

		if (!GetComponent<Player>().breathingSound.isPlaying) {
            recharging = false;
        }

        switch (state) {
            case PlayerState.WAKE:
                Wake();
                break;
            case PlayerState.STAND:
                Stand();
                break;
            case PlayerState.WALK:
                Walk();
                break;
            case PlayerState.CROUCH:
                Crouch();
                break;
            case PlayerState.SPRINT:
                Sprint();
                break;
            case PlayerState.FALL:
                Fall();
                break;
        }
	}

    void Wake() {
        SetMovementSpeed(0f);

        //change state when animation ends
        if (!this.gameObject.animation.IsPlaying("OpeningCut")) {
            state = PlayerState.STAND;
        }
    }

    void Stand() 
	{
        SetMovementSpeed(0f);

        //do nothing unless state should change
        if (Falling()) {
            state = PlayerState.FALL;
        } 
		/*
		else if (Crouching()) 
		{
            state = PlayerState.CROUCH;
        } 
        */
		else if (Sprinting()) {
            state = PlayerState.SPRINT;
        } else if (Moving()) {
            state = PlayerState.WALK;
        }
    }

    void Walk() {
        if (!walking) 
		{
			//Walking speed
			if(inWater)
			{
				SetMovementSpeed(3.0F);
			}
			else
			{
            	SetMovementSpeed(6f);
			}
        }

		if(GameObject.Find("Main Camera").GetComponent<cameraShake>().shake == false
		   && !this.gameObject.animation.IsPlaying("Landing"))
		{
        	this.gameObject.animation.Play("Walk");
		}

        if (Falling()) 
		{
            //if falling, change state
            state = PlayerState.FALL;
        } 
		else if (Crouching()) 
		{
            //if crouching, change state
            state = PlayerState.CROUCH;
        } 
		else if (Sprinting()) 
		{
            //if sprinting, change state
            state = PlayerState.SPRINT;
        } 
		else if (!Moving()) 
		{
            //if not moving, change state
            state = PlayerState.STAND;
        }
    }

    void Crouch() 
	{
        if(canCrouch)
        {
			Debug.Log(canCrouch);
            if (!crouching)
            {
                SetMovementSpeed(3f);

                SetCrouch(true);
            }

            if (Falling())
            {
                //if falling, change state
                state = PlayerState.FALL;

                SetCrouch(false);
            }
            else if (!Crouching())
            {
                //if not crouching, change state
                state = PlayerState.STAND;

                SetCrouch(false);
            }
        }
        else
        {
            state = PlayerState.STAND;
        }
    }

    void Sprint() 
	{
        if(!sprinting) 
		{
			if(inWater)
			{
				SetMovementSpeed(6.0F);
			}
			else
			{
				//Sprint speed
	            SetMovementSpeed(12.0F);
			}
        }

        if (Input.GetKey(KeyCode.W)) 
		{
            if (sprintTime > 0) 
			{
                //if moving forward, play animation and update timer
				if(GameObject.Find("Main Camera").GetComponent<cameraShake>().shake == false
				   && !this.gameObject.animation.IsPlaying("Landing"))
				{
                	this.gameObject.animation.Play("Sprint");
				}

                sprintTime -= Time.deltaTime;
            } 
			else 
			{
                //if timer under 0, change state
                state = PlayerState.STAND;

                breathing = true;
                recharging = true;
            }
        } else 
		{
            //if not moving, change state
            state = PlayerState.STAND;

            breathing = true;
            recharging = true;
        }

        if (Falling()) 
		{
            //if falling, change state
            state = PlayerState.FALL;
        } 
		else if(!Sprinting()) 
		{
            //if not sprinting, change state
            state = PlayerState.STAND;

            breathing = true;
            recharging = true;
        }
    }

    void Fall() 
	{
        if (motor.IsGrounded()) 
		{
			if(GameObject.Find("Main Camera").GetComponent<cameraShake>().shake == false)
			{
            	this.gameObject.animation.Play("Landing");
			}

			//if hit ground, change state
			state = PlayerState.STAND;
        }
    }

    void SetMovementSpeed(float speed) 
	{
        motor.movement.maxForwardSpeed = speed;
        motor.movement.maxSidewaysSpeed = speed;
        motor.movement.maxBackwardsSpeed = speed;

        walking = state == PlayerState.WALK;
        crouching = state == PlayerState.CROUCH;
        sprinting = state == PlayerState.SPRINT;
    }

	bool Falling()
	{
		if(!motor.IsGrounded()) 
		{
			return true;
		}

		return false;
	}

    bool Crouching() {
        return (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand));
    }

    bool Sprinting() {
        return (Input.GetKey(KeyCode.LeftShift) && sprintTime > 0 && !recharging);
    }

    bool Moving() {
        return (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));
    }

	/*
	bool Falling()
	{
		//Player layer mask
		int playerLayer = 8;
		int invisibleLayer = 10;
		int ignoreMask = 1 << playerLayer | 1 << invisibleLayer;
		
		//Invert bitmask to only ignore this layer
		ignoreMask = ~ignoreMask;

		RaycastHit hit;
		Debug.DrawRay(transform.position, Vector3.down * 2.0F, Color.yellow);
		if(Physics.Raycast(transform.position, Vector3.down, out hit, 2.0F, ignoreMask))
		{
			Debug.Log(hit.collider.name);
			return false;
		}
		else
		{
			Debug.Log(hit.collider.name);
			return true;
		}
	}*/

    void SetCrouch(bool isCrouching) 
	{
        if (!isCrouching) {
            //alter collider height
            box.size = new Vector3(0, boxStandHeight, 0);
            controller.height = boxStandHeight;

            //alter camera height
            Vector3 cameraPos = camera.transform.localPosition;
            camera.transform.localPosition = new Vector3(cameraPos.x, cameraStandHeight, cameraPos.z);

            //move player to ground level
            Vector3 playerPos = gameObject.transform.position;
            gameObject.transform.position = new Vector3(playerPos.x, playerPos.y + 1f, playerPos.z);
        } else {
            //alter collider height
            box.size = new Vector3(0, boxCrouchHeight, 0);
            controller.height = boxCrouchHeight;

            //alter camera height
            Vector3 cameraPos = camera.transform.localPosition;
            camera.transform.localPosition = new Vector3(cameraPos.x, cameraCrouchHeight, cameraPos.z);

            //move player to ground level
            Vector3 playerPos = gameObject.transform.position;
            gameObject.transform.position = new Vector3(playerPos.x, playerPos.y - 1f, playerPos.z);
        }
    }

    void OnTriggerEnter(Collider col)
    {
		if(col.name == "WaterZone")
		{
			inWater = true;
			SetMovementSpeed(motor.movement.maxForwardSpeed/2);
		}
        if(col.name == "WaterNoJump")
		{
			inWater = true;
			motor.jumping.enabled = false;
			SetMovementSpeed(motor.movement.maxForwardSpeed/2);
		}
    }

    void OnTriggerExit(Collider col)
    {
		if(col.name == "WaterZone")
		{
			inWater = false;
			SetMovementSpeed(motor.movement.maxForwardSpeed*2);
		}
        if (col.name == "WaterNoJump")
		{
			inWater = false;
			motor.jumping.enabled = true;
			SetMovementSpeed(motor.movement.maxForwardSpeed*2);
		}
    }
}

public enum PlayerState { WAKE, STAND, WALK, CROUCH, SPRINT, FALL };