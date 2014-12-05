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
    public bool canCrouch = true;
    //used to play breathing sound
    private bool breathing;
    public AudioSource breathingSound;

    //player motor and controller
    public CharacterMotor motor;
    public CharacterController controller;

	// Use this for initialization
	void Start () {
        //set timer
        sprintTime = 5f;
        maxSprintTime = 5f;

        //set player collider heights
        boxStandHeight = box.size.y;
        boxCrouchHeight = 1.125f;

        //set player camera heights
        cameraStandHeight = camera.transform.localPosition.y;
        cameraCrouchHeight = 0.30857f;
        
		AudioSource[] playerSounds = transform.Find("Graphics").GetComponents<AudioSource>(); //Grab the audio sources on the graphics child
		breathingSound = playerSounds[1];;
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

                breathingSound.Play();
            }
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

    void Stand() {
        SetMovementSpeed(0f);

        //do nothing unless state should change
        if (Falling()) {
            state = PlayerState.FALL;
        } else if (Crouching()) {
            state = PlayerState.CROUCH;
        } else if (Sprinting()) {
            state = PlayerState.SPRINT;
        } else if (Moving()) {
            state = PlayerState.WALK;
        }
    }

    void Walk() {
        if (!walking) {
            SetMovementSpeed(6f);
        }

        this.gameObject.animation.Play("Walk");

        if (Falling()) {
            //if falling, change state
            state = PlayerState.FALL;
        } else if (Crouching()) {
            //if crouching, change state
            state = PlayerState.CROUCH;
        } else if (Sprinting()) {
            //if sprinting, change state
            state = PlayerState.SPRINT;
        } else if (!Moving()) {
            //if not moving, change state
            state = PlayerState.STAND;
        }
    }

    void Crouch() {
        if (canCrouch)
        {
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

    void Sprint() {
        if (!sprinting) {
            SetMovementSpeed(12f);
        }

        if (Input.GetKey(KeyCode.W)) {
            if (sprintTime > 0) {
                //if moving forward, play animation and update timer
                this.gameObject.animation.Play("Sprint");

                sprintTime -= Time.deltaTime;
            } else {
                //if timer under 0, change state
                state = PlayerState.STAND;

                breathing = true;
            }
        } else {
            //if not moving, change state
            state = PlayerState.STAND;

            breathing = true;
        }

        if (Falling()) {
            //if falling, change state
            state = PlayerState.FALL;
        } else if (!Sprinting()) {
            //if not sprinting, change state
            state = PlayerState.STAND;

            breathing = true;
        }
    }

    void Fall() {
        if (motor.IsGrounded()) {
            //if hit ground, change state
            state = PlayerState.STAND;

            this.gameObject.animation.Play("Landing");
        }
    }

    void SetMovementSpeed(float speed) {
        motor.movement.maxForwardSpeed = speed;
        motor.movement.maxSidewaysSpeed = speed;
        motor.movement.maxBackwardsSpeed = speed;

        walking = state == PlayerState.WALK;
        crouching = state == PlayerState.CROUCH;
        sprinting = state == PlayerState.SPRINT;
    }

    bool Crouching() {
        return (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand));
    }

    bool Sprinting() {
        return (Input.GetKey(KeyCode.LeftShift) && sprintTime > 0);
    }

    bool Moving() {
        return (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));
    }

    bool Falling() {
        return (Input.GetKey(KeyCode.Space));
    }

    void SetCrouch(bool isCrouching) {
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
        if (col.name == "PoolNoCrouch")
            canCrouch = false;
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "PoolNoCrouch")
            canCrouch = true;
    }
}

public enum PlayerState { WAKE, STAND, WALK, CROUCH, SPRINT, FALL };