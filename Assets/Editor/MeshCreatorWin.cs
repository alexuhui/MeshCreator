using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshCreatorWin : EditorWindow
{
    [MenuItem("Tools/MeshCreator")]
    private static void ShowWin()
    {
        var win = GetWindow<MeshCreatorWin>("MeshCreator");
        win.Show();
    }

    private Creator[] creators = {
        new RectMeshCreator(),
    };

    private void OnEnable()
    {
        for (int i = 0; i < creators.Length; i++)
        {
            creators[i].OnEnable();
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < creators.Length; i++)
        {
            creators[i].OnDisable();
        }
    }

    private void OnGUI()
    {
        if(creators == null || creators.Length == 0)
            GUILayout.Label("Î´×¢²áÉú³ÉÆ÷");

        for (int i = 0; i < creators.Length; i++)
        {
            creators[i].OnGUI();
        }
    }
}
