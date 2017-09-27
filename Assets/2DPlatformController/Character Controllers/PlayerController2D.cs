using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerController2D : CharacterController2D
{
    [SerializeField] private Transform _bodySprite;

    private PlayerInputManager _inputManager;

    protected override void Start()
    {
        base.Start();
        _inputManager = GetComponent<PlayerInputManager>();
    }

    protected override void Update()
    {
        Vector2 moveInput = _inputManager.GetMovementInput();
        Vector2 movemenetDelta = ProcessMovementInput(moveInput);
        ApplyMovement(movemenetDelta);
    }

    protected override void OnCharacterTurn(int turnDir)
    {
        Vector3 currRot = _bodySprite.rotation.eulerAngles;
        _bodySprite.rotation = Quaternion.Euler(currRot.x, turnDir > 0 ? 0 : 180, currRot.z);
        base.OnCharacterTurn(turnDir);
    }
}
