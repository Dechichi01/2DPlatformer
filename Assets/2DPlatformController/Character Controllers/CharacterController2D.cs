using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(PhysicsController2D))]
public abstract class CharacterController2D : MonoBehaviour {

    [SerializeField] private PhysicsController2D controller;
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float accTimeGround = .1f;
    [SerializeField] private float accTimeAir = .2f;
    [SerializeField] private float jumpHeight = 3.5f;
    [SerializeField] private float timeToJumpApex = .6f;
    [SerializeField] [Range(0, 1)] private float doubleJumpRatio = .8f;

    private float gravity;
    private float jumpVelocity;
    private float velocityXSmooth;
    protected Vector2 currVelocity;

    protected CharacterState _state = new CharacterState();

    protected Queue<CharacterActions> _actionsQueue = new Queue<CharacterActions>();

    public BoxCollider2D BoxCollider { get { return controller.Collider2D; } }

    protected virtual void Start ()
    {
        CalculateGravityAndJumpVelocity();
    }

    protected abstract void Update();

    protected virtual void OnEnable()
    {
        controller.OnChangeDirectionX += OnCharacterTurn;
    }

    protected virtual void OnDisable()
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
        if (!_state.CanMove)
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
            else if (_state.CanJump() || _state.CanDoubleJump())
            {
                currVelocity.y = movementInput.y * jumpVelocity;
                _state.JumpCount++;
            }
        }

        return currVelocity * Time.deltaTime;
    }

    protected virtual void ApplyMovement(Vector2 deltaMovement)
    {
        controller.Move(deltaMovement);
    }

    protected virtual void ProcessActionQueue(ref Vector2 inputValue)
    {
        if (_actionsQueue.Count == 0)
            return;

        switch (_actionsQueue.Dequeue())
        {
            case CharacterActions.Jump:
                if (_state.CanJump())
                    Jump(ref inputValue);
                else if (_state.CanDoubleJump())
                    DoubleJump(ref inputValue);
                break;                
            case CharacterActions.Shoot:
                Shoot();
                break;
            case CharacterActions.Charge:
                Charge();
                break;
            case CharacterActions.Special:
                PerformSpecial();
                break;
        }
    }

    protected virtual void OnCharacterTurn(int turnDir)
    {
        _state.Facing = turnDir;
    }

    public void OnInspectorValuesChanged()
    {
        CalculateGravityAndJumpVelocity();
    }

    #region CharacterActions
    protected virtual void Jump(ref Vector2 inputValue)
    {
        inputValue.y = 1;
    }

    protected virtual void DoubleJump(ref Vector2 inputValue)
    {
        inputValue.y = doubleJumpRatio;
    }

    protected virtual void Shoot()
    {

    }

    protected virtual void Charge()
    {

    }

    protected virtual void PerformSpecial()
    {

    }
    #endregion

    [System.Serializable]
    protected class CharacterState
    {
        public bool CanMove = true;
        public int Facing = 1;
        public bool Grounded
        {
            get
            {
                return _grounded;
            }
            set
            {
                _grounded = value;
                if (_grounded)
                    JumpCount = 0;
            }
        }

        private bool _grounded = false;
        public int JumpCount;

        public bool CanJump()
        {
            return Grounded && JumpCount == 0;
        }

        public bool CanDoubleJump()
        {
            return !Grounded && JumpCount == 1;
        }
    }

    protected enum CharacterActions
    {
        Jump = 0, Shoot = 1, Charge = 2, Special = 3
    }
}


