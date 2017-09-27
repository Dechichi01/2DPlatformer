using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(PhysicsController2D))]
public abstract class CharacterController2D : MonoBehaviour {

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float accTimeGround = .1f;
    [SerializeField] private float accTimeAir = .2f;
    [SerializeField] private float jumpHeight = 3.5f;
    [SerializeField] private float timeToJumpApex = .6f;

    private float gravity;
    private float jumpVelocity;
    private float velocityXSmooth;
    protected Vector2 currVelocity;

    protected PhysicsController2D controller;
    protected CharacterState _state = new CharacterState();

    public BoxCollider2D BoxCollider { get { return controller.Collider2D; } }

    private void Awake()
    {
        controller = GetComponent<PhysicsController2D>();
    }

    protected virtual void Start ()
    {
        CalculateGravityAndJumpVelocity();
    }

    protected abstract void Update();

    private void OnEnable()
    {
        controller.OnChangeDirectionX += OnCharacterTurn;
    }

    private void OnDisable()
    {
        controller.OnChangeDirectionX -= OnCharacterTurn;
    }

    private void CalculateGravityAndJumpVelocity()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    protected Vector2 ProcessMovementInput(Vector2 movementInput)
    {
        if (!_state.CanPerformAction || !_state.CanMove)
            movementInput = Vector2.zero;

        //Calculate velocity.x
        float targetVX = moveSpeed * movementInput.x;
        currVelocity.x = Mathf.SmoothDamp(currVelocity.x, targetVX, ref velocityXSmooth, _state.Grounded ? accTimeGround : accTimeAir);
        
        //Calculate velocity.y
        _state.Grounded = controller.collisions.below;

        //Not accumulate gravity
        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
                currVelocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            else
                currVelocity.y = 0;
        }

        currVelocity.y += gravity * Time.deltaTime;

        if (movementInput.y > 0)//Jump
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                if (movementInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))//not jumping againt max slope
                {
                    currVelocity.y = jumpVelocity * controller.collisions.slopeNormal.y;
                    currVelocity.x = jumpVelocity * controller.collisions.slopeNormal.x;
                }
            }
            else if (_state.Grounded)
            {
                currVelocity.y = movementInput.y * jumpVelocity;
            }
        }

        return currVelocity * Time.deltaTime;
    }

    protected virtual void ApplyMovement(Vector2 deltaMovement)
    {
        controller.Move(deltaMovement);
    }

    protected virtual void OnCharacterTurn(int turnDir)
    {
        _state.Facing = turnDir;
    }

    public void OnInspectorValuesChanged()
    {
        CalculateGravityAndJumpVelocity();
    }

    [System.Serializable]
    protected class CharacterState
    {
        public bool CanMove = true;
        public bool CanPerformAction = true;
        public int Facing = 1;
        public bool Grounded = true;

    }
}


