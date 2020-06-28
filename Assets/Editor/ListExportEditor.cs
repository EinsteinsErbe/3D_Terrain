using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ListExport))]
public class ListExportEditor : Editor
{
    ListExport exporter;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Export"))
        {
            exporter.ExportList();
        }
    }

    void OnEnable()
    {
        exporter = (ListExport)target;
        exporter.Init();
        Tools.hidden = true;
    }

    void OnDisable()
    {
        Tools.hidden = false;
    }
}
