using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimController : MonoBehaviour {

    [SerializeField] private Animator _anim;

    private bool _grounded;

    public void SetMovement(float xMovement, Vector3 right)
    {
        float sign = Mathf.Sign(Vector3.Dot(right, Vector3.right));
        xMovement = Mathf.Clamp(xMovement, -1, 1);
        _anim.SetFloat(AnimConstants.MovementX, sign * xMovement);
    }

    public void Jump()
    {
        _anim.SetTrigger(AnimConstants.Jump);
        SetOnGround(true);
    }

    public void SetOnGround(bool value)
    {
        if (_grounded != value)
        {
            _grounded = value;
            _anim.SetBool(AnimConstants.Grounded, _grounded);
        }
    }

    private class AnimConstants
    {
        public const string MovementX = "movementX";
        public const string Jump = "jump";
        public const string Grounded = "grounded";
    }
}
