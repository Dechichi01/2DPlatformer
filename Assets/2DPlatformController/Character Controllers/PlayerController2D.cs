using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerController2D : CharacterController2D
{
    [SerializeField] private Transform _bodySprite;
    [SerializeField] private PlayerAim _playerAim;
    [SerializeField] private CharacterAnimController _animCtrl;
     
    private PlayerInputManager _inputManager;

    private void Awake()
    {
        _inputManager = GetComponent<PlayerInputManager>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _inputManager.JumpButton.OnPress += OnJumpButtonPressed;
        _inputManager.ShootButton.OnHold += OnShootButtonHold;
        _inputManager.ShootButton.OnRelease += OnShootButtonReleased;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _inputManager.JumpButton.OnPress -= OnJumpButtonPressed;
        _inputManager.ShootButton.OnHold -= OnShootButtonHold;
        _inputManager.ShootButton.OnRelease -= OnShootButtonReleased;
    }

    protected override void Update()
    {
        Vector2 joystickInput= _inputManager.GetJoystickInput();
        Vector2 movementInput = new Vector2(joystickInput.x, 0);
        ProcessActionQueue(ref movementInput);        

        Vector2 movementDelta = ProcessMovementInput(movementInput);
        ApplyMovement(movementDelta);

        _animCtrl.SetMovement(movementDelta.x / Time.deltaTime, _bodySprite.right);
        _animCtrl.SetOnGround(_state.Grounded);

        float aimAngle = joystickInput.x == 0 ? 0 : Mathf.Atan(joystickInput.y/joystickInput.x)*Mathf.Rad2Deg;
        _playerAim.SetAimRotation(aimAngle);
    }

    protected override void OnCharacterTurn(int turnDir)
    {
        Vector3 currRot = _bodySprite.rotation.eulerAngles;
        _bodySprite.rotation = Quaternion.Euler(currRot.x, turnDir > 0 ? 0 : 180, currRot.z);
        base.OnCharacterTurn(turnDir);
    }

    private void OnJumpButtonPressed()
    {
        _actionsQueue.Enqueue(CharacterActions.Jump);
    }

    private void OnShootButtonHold()
    {
        _actionsQueue.Enqueue(CharacterActions.Charge);
    }

    private void OnShootButtonReleased()
    {
        _actionsQueue.Enqueue(CharacterActions.Shoot);
    }

    protected override void Jump(ref Vector2 inputValue)
    {
        base.Jump(ref inputValue);
        _animCtrl.Jump();
    }
}
