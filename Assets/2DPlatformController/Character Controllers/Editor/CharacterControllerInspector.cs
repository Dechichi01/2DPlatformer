using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterController2D), true)]
public class CharacterControllerInspector : Editor {

    public override void OnInspectorGUI()
    {
        if (DrawDefaultInspector())
        {
            CharacterController2D _char = (CharacterController2D)target;
            _char.OnInspectorValuesChanged();
        }
    }
}
