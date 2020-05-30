using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(MapExport))]
public class MapExportEditor : Editor
{
    MapExport exporter;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Export"))
        {
            exporter.ExportTile();
        }
        
        if (GUILayout.Button("Export Real"))
        {
            exporter.ExportReal();
        }

        if (GUILayout.Button("Export Generated"))
        {
            exporter.ExportGenerated();
        }
    }

    void OnEnable()
    {
        exporter = (MapExport)target;
        exporter.Init();
        Tools.hidden = true;
    }

    void OnDisable()
    {
        Tools.hidden = false;
    }
}
