using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TileLoader))]
public class TileEditor : Editor
{
    TileLoader loader;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Load Tile"))
        {
            loader.LoadTile();
        }

        if (GUILayout.Button("Load Random"))
        {
            loader.LoadRandomTile();
        }
    }

    void OnEnable()
    {
        loader = (TileLoader)target;
        loader.Init();
        Tools.hidden = true;
    }

    void OnDisable()
    {
        Tools.hidden = false;
    }
}
