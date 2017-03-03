using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ChamberBuilderScript))]
public class ChamberBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChamberBuilderScript myScript = (ChamberBuilderScript) target;
        if (GUILayout.Button("Bake!"))
        {
            myScript.BuildObject();
        }
    }
}