using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TerrainLoader))]
public class TerrainEditor : Editor
{
    TerrainLoader loader;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Mesh"))
        {
            loader.Init();
        }
    }

    void OnEnable()
    {
        loader = (TerrainLoader)target;
        Tools.hidden = true;
    }

    void OnDisable()
    {
        Tools.hidden = false;
    }
}
