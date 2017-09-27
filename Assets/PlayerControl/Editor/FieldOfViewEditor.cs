using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FieldOfView2D))]
public class FieldOfViewEditor : Editor
{

    void OnSceneGUI()
    {
        FieldOfView2D fow = (FieldOfView2D)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.right, 360, fow.viewRadius);
        Vector3 viewDirA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewDirB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewDirA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewDirB * fow.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fow.visibleTargets)
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
    }

}
